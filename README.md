﻿# spidey-senses
## Infos
SpiderControl.Core&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-> Contains main functionality and definitions  
SpiderControl.Application&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-> Combines everything together from SpiderControl.Core to process all commands  
SpiderControl.Console&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;-> Main entry point - uses SpiderControl.Application with the arguments you supply it  

## experimental
This was fun, so I'm continuing dev work in the experimental branch. Next steps:
1. Simple angular web UI to show the spider on the grid.
2. Web API to get/use results from SpiderControl.Application to move the web UI spider around.
3. Docker
4. Kubernetes
