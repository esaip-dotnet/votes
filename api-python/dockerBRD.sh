#/bin/sh
clear
sudo docker rm -fv `sudo docker ps -aq` 
sudo docker build -t test .
sudo docker run -d -p 80:5000 --name Test test
sudo docker logs Test
