# git-bin

Why it's awesome.


## Configuration

Step 1: Add a new filter called 'bin'

```bash
$ git config --global filter.bin.clean “git bin clean %f”
$ git config --global filter.bin.smudge "git bin smudge"
```

Step 2: Supply your Amazon S3 information

```bash
$ git config --global git-bin.s3bucket "your bucket name"
$ git config --global git-bin.s3key "your key"
$ git config --global git-bin.s3secretKey "your secret key"
```

Step 3: Set up .gitattributes

Create or edit the .gitattributes file that is in the root of the git repo that you want to use git-bin with. Specify file extensions that should be run through git-bin, for example:

```
*.jpg filter=bin binary
*.png filter=bin binary
*.bmp filter=bin binary
*.psd filter=bin binary
etc, etc, etc
```


## Usage

Once the three configuration steps have been completed, normal use of git will invoke git-bin for files that match a pattern in .gitattributes.

Adding a new file (the JPG is 217k, but only 409b were sent to git):

```bash
$ ls -lh brooklyn_bridge.jpg
217k Dec 20 15:03 brooklyn_bridge.jpg
 
$ git add brooklyn_bridge.jpg
[git-bin] Cleaning brooklyn_bridge.jpg

$ git commit -m "Added JPG that's manage by git-bin"
[dev 6adb9c6] Added JPG that's manage by git-bin
 1 files changed, 0 insertions(+), 0 deletions(-)
 create mode 100644 brooklyn_bridge.jpg
 
$ git bin push
[git-bin] Uploading 1 chunks...
  [0/1] -> 0..10..20..30..40..50..60..70..80..90..100

$ git push
Counting objects: 4, done.
Delta compression using up to 2 threads.
Compressing objects: 100% (3/3), done.
Writing objects: 100% (3/3), 409 bytes, done.
Total 3 (delta 1), reused 0 (delta 0)
To git@github.com:xxxx/MyRepo.git
   dc53749..6adb9c6  master -> master
[git-bin] Cleaning brooklyn_bridge.jpg
```

When you or someone else on the team checks out a file it will either be pulled from the on-disk cache or downloaded if it's not in the cache:

```bash
$ git pull
remote: Counting objects: 4, done.
remote: Compressing objects: 100% (2/2), done.
remote: Total 3 (delta 1), reused 3 (delta 1)
Unpacking objects: 100% (3/3), done.
From github.com:xxxx/MyRepo
   dc53749..6adb9c6  master     -> origin/master
Updating dc53749..6adb9c6
[git-bin] Smudging brooklyn_bridge.jpg... Downloading 1 chunks...
        [0/1] -> 0..10..20..30..40..50..60..70..80..90..100
Fast-forward
 brooklyn_bridge.jpg |  Bin 0 -> 222616 bytes
 1 files changed, 0 insertions(+), 0 deletions(-)
 create mode 100644 brooklyn_bridge.jpg
```


## Optional configuration


## How it works



[Optional]
chunkSize
maxCacheSize

core.largefilethreshold