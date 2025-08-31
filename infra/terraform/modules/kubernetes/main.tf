provider "kubernetes" {
  host                   = var.host
  client_certificate     = var.client_certificate
  client_key             = var.client_key
  cluster_ca_certificate = var.cluster_ca_certificate
}

provider "helm" {
  kubernetes {
    host                   = var.host
    client_certificate     = var.client_certificate
    client_key             = var.client_key
    cluster_ca_certificate = var.cluster_ca_certificate
  }
}

# Create a namespace for the application
resource "kubernetes_namespace" "spidey" {
  metadata {
    name = "spidey-senses-${var.environment}"
  }
}

# Create a secret for GitHub Container Registry
resource "kubernetes_secret" "github_registry" {
  metadata {
    name      = "github-registry"
    namespace = kubernetes_namespace.spidey.metadata[0].name
  }

  type = "kubernetes.io/dockerconfigjson"

  data = {
    ".dockerconfigjson" = jsonencode({
      auths = {
        "ghcr.io" = {
          auth = base64encode("github:${var.github_token}")
        }
      }
    })
  }
}

# Deploy the API
resource "kubernetes_deployment" "api" {
  metadata {
    name      = "api"
    namespace = kubernetes_namespace.spidey.metadata[0].name
    labels = {
      app = "api"
    }
  }

  spec {
    replicas = var.api_replicas

    selector {
      match_labels = {
        app = "api"
      }
    }

    template {
      metadata {
        labels = {
          app = "api"
        }
      }

      spec {
        container {
          image = var.api_image_name
          name  = "api"

          port {
            container_port = 8080
          }

          resources {
            limits = {
              cpu    = "500m"
              memory = "512Mi"
            }
            requests = {
              cpu    = "250m"
              memory = "256Mi"
            }
          }

          env {
            name  = "ASPNETCORE_ENVIRONMENT"
            value = var.environment == "dev" ? "Development" : "Production"
          }

          # liveness_probe {
          #   http_get {
          #     path = "/api/health/liveness"
          #     port = 8080
          #   }
          #   initial_delay_seconds = 30
          #   period_seconds        = 10
          # }

          # readiness_probe {
          #   http_get {
          #     path = "/api/health/readiness"
          #     port = 8080
          #   }
          #   initial_delay_seconds = 5
          #   period_seconds        = 10
          # }
        }

        image_pull_secrets {
          name = kubernetes_secret.github_registry.metadata[0].name
        }
      }
    }
  }
}

# Service for the API
resource "kubernetes_service" "api" {
  metadata {
    name      = "api"
    namespace = kubernetes_namespace.spidey.metadata[0].name
  }

  spec {
    selector = {
      app = kubernetes_deployment.api.metadata[0].labels.app
    }

    port {
      port        = 80
      target_port = 8080
    }

    type = "ClusterIP"
  }
}

# Deploy the UI
resource "kubernetes_deployment" "ui" {
  metadata {
    name      = "frontend"
    namespace = kubernetes_namespace.spidey.metadata[0].name
    labels = {
      app = "frontend"
    }
  }

  spec {
    replicas = var.ui_replicas

    selector {
      match_labels = {
        app = "frontend"
      }
    }

    template {
      metadata {
        labels = {
          app = "frontend"
        }
      }

      spec {
        container {
          image = var.ui_image_name
          name  = "ui"

          port {
            container_port = 80
          }

          resources {
            limits = {
              cpu    = "300m"
              memory = "256Mi"
            }
            requests = {
              cpu    = "100m"
              memory = "128Mi"
            }
          }

          env {
            name  = "ANGULAR_ENV"
            value = var.environment
          }

          liveness_probe {
            http_get {
              path = "/"
              port = 80
            }
            initial_delay_seconds = 30
            period_seconds        = 10
          }
        }
        
        image_pull_secrets {
          name = kubernetes_secret.github_registry.metadata[0].name
        }
      }
    }
  }
}

# Service for the UI
resource "kubernetes_service" "ui" {
  metadata {
    name      = "frontend"
    namespace = kubernetes_namespace.spidey.metadata[0].name
  }

  spec {
    selector = {
      app = kubernetes_deployment.ui.metadata[0].labels.app
    }

    port {
      port        = 80
      target_port = 80
    }

    type = "ClusterIP"
  }
}

# Install Ingress Controller using Helm
resource "helm_release" "nginx_ingress" {
  name       = "nginx-ingress"
  repository = "https://kubernetes.github.io/ingress-nginx"
  chart      = "ingress-nginx"
  namespace  = kubernetes_namespace.spidey.metadata[0].name
  
  set {
    name  = "controller.replicaCount"
    value = "2"
  }
  
  set {
    name  = "controller.service.externalTrafficPolicy"
    value = "Local"
  }
}

# Data source to fetch the Ingress Controller service
data "kubernetes_service" "ingress_controller" {
  metadata {
    name      = "nginx-ingress-ingress-nginx-controller"
    namespace = kubernetes_namespace.spidey.metadata[0].name
  }
  
  depends_on = [helm_release.nginx_ingress]
}