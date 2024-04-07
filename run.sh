# Create image
sudo docker build -t nekodownloader:latest .

# Test image
# sudo docker run --rm -it -v mangas:/mnt/mangas nekodownloader:latest ls -l /mnt/media