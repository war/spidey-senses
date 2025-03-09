# spidey-senses - (Howden - Driving Data take home test)
## Infos
SpiderControl.Core&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-> Contains main functionality and definitions  
SpiderControl.Application&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-> Combines everything together from SpiderControl.Core to process all commands  
SpiderControl.Console&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-> Main entry point - uses SpiderControl.Application with the arguments you supply it  

This was fun, so I'm continuing work on this. 
Next steps:

0. Optimise some bits and pieces that I didn't get to finish - done
1. Simple angular web UI to show the spider on the grid/wall - done
2. Web API v2 to get/use results from SpiderControl.Application to move the web UI spider around - done
3. GraphQL with Hot Chocolate (2nd API implementation)
4. gRPC (3rd API implementation)
5. Minimal APIs (4th API implementation) - in progress
6. FastEndpoints (5th API implementation) - in progress
7. Serilog logging - in progress
8. Docker - done
9. Docker-compose - done
10. Kubernetes
11. CI/CD - done (build, push to github repo, code coverage, test, healthcheck)
12. Process spider movements, but show movement history on grid - in progress
