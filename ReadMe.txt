--docker volume
--WINDOWS Containers
docker volume create updateresxfilevolume

--build image. Make sure you are in dockerfile folder.
docker build -t updateresxfile .

-- run the image by creating container and mounting the volume
docker run -d -p 1401:80 --name updateresxfilecontainer1 -v updateresxfilevolume:C:/Resxfiles updateresxfile