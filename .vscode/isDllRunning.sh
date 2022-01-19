#!/bin/bash

while [ ! $(docker exec backend ps -aux | pgrep -x "API") > /dev/null  ]
do
sleep 0.1
done