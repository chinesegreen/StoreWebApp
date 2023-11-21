using Amazon.S3;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageSaver : IImageSaver
    {
        public void Index()
        {
            AmazonS3Config configsS3 = new AmazonS3Config
            {
                ServiceURL = "https://s3.yandexcloud.net"
            };

            AmazonS3Client s3client = new AmazonS3Client(configsS3);

            //var response = s3client.GetBucketRequestPaymentAsync();
        }
    }
}
