# flickr-uploader

Console application supporting upload of images stored on local storage to Flickr. The application has been developed as a reaction to Flickr's move to make a [Desktop Auto-Uploadr](https://www.flickr.com/tools/) tool [a paid feature](http://www.theverge.com/2016/3/9/11184518/flickr-photo-uploader-now-paid-feature). This app does the very minimum I needed to be able to keep using Flickr as a backup medium in my image processing workflow.

## features
* uploads images (JPEG) in full resolution to Flickr account
* creates "Sets" based on the folder name the image is stored in
* does not duplicate photos already existing in the current photoset
* supports deep folder hierarchy
* works on both linux/windows operating systems

## setup

Following steps needs to be done only for the very first time. 

1. go ahead, download and unzip the latest [release version](https://github.com/luk355/flickr-uploader/releases) of `flick-uploader-cli.zip` from GitHub
1. navigate to [flickr app garden](https://www.flickr.com/services/apps/create/apply) and apply for a non non commercial key
1. navigate to the unzipped application and modify `appsettings.json` config file
    * copy a Key and Secret values from flickr app you created in step 2
        * Key -> flickrApiKey 
        * Secret -> flickrSecret
    * specify the root folder of the photos you want to upload in photoPath key

## usage

Navigate to the unzipped archive created in setup section and run app in console.

```Shell
$ ./FlickrUploader.Console.exe
Please provide authentication code:
```

The application will open the Flickr website in the browser asking for app authorization when run for the first time. The application will wait until you copy the auhorization code shown in the browser to the console.

The app can be re-run anytime new photos are added to the folder. Only new photos will be uploaded.

# disclaimer

This app has been developed mainly for my personal use therefore I do not guarantee it will work for your needs. However, I am happy to hear any suggestions, comments. Any help is more than welcome.
