/*
 * Copyright (C) Alibaba Cloud Computing
 * All rights reserved.
 * 
 */
using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MR.Utility.Aliyun.OSS
{
    /// <summary>
    /// 分片上传文件至阿里云OSS
    /// </summary>
    public class MultipartUpload
    {
        static string accessKeyId = Config.AliyunOSS.AccessKeyId;
        static string accessKeySecret = Config.AliyunOSS.AccessKeySecret;
        static string endpoint = Config.AliyunOSS.Endpoint;
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        static string key = "key-1";
        static string fileToUpload = Config.AliyunOSS.BigFileToUpload;

        static int partSize = 50 * 1024 * 1024;

        /// <summary>
        /// UploadPartContext
        /// </summary>
        public class UploadPartContext
        {
            /// <summary>
            /// BucketName
            /// </summary>
            public string? BucketName { get; set; }
            /// <summary>
            /// ObjectName
            /// </summary>
            public string? ObjectName { get; set; }
            /// <summary>
            /// PartETags
            /// </summary>
            public List<PartETag>? PartETags { get; set; }
            /// <summary>
            /// UploadId
            /// </summary>
            public string? UploadId { get; set; }
            /// <summary>
            /// TotalParts
            /// </summary>
            public long TotalParts { get; set; }
            /// <summary>
            /// CompletedParts
            /// </summary>
            public long CompletedParts { get; set; }
            /// <summary>
            /// SyncLock
            /// </summary>
            public object? SyncLock { get; set; }
            /// <summary>
            /// WaitEvent
            /// </summary>
            public ManualResetEvent? WaitEvent { get; set; }
        }

        /// <summary>
        /// UploadPartContextWrapper
        /// </summary>
        public class UploadPartContextWrapper
        {
            /// <summary>
            /// Context
            /// </summary>
            public UploadPartContext Context { get; set; }
            /// <summary>
            /// PartNumber
            /// </summary>
            public int PartNumber { get; set; }
            /// <summary>
            /// PartStream
            /// </summary>
            public Stream PartStream { get; set; }

            /// <summary>
            /// UploadPartContextWrapper
            /// </summary>
            /// <param name="context"></param>
            /// <param name="partStream"></param>
            /// <param name="partNumber"></param>
            public UploadPartContextWrapper(UploadPartContext context, Stream partStream, int partNumber)
            {
                Context = context;
                PartStream = partStream;
                PartNumber = partNumber;
            }
        }

        /// <summary>
        /// UploadPartCopyContext
        /// </summary>
        public class UploadPartCopyContext
        {
            /// <summary>
            /// TargetBucket
            /// </summary>
            public string? TargetBucket { get; set; }
            /// <summary>
            /// TargetObject
            /// </summary>
            public string? TargetObject { get; set; }
            /// <summary>
            /// PartETags
            /// </summary>
            public List<PartETag>? PartETags { get; set; }
            /// <summary>
            /// UploadId
            /// </summary>
            public string? UploadId { get; set; }
            /// <summary>
            /// TotalParts
            /// </summary>
            public long TotalParts { get; set; }
            /// <summary>
            /// CompletedParts
            /// </summary>
            public long CompletedParts { get; set; }
            /// <summary>
            /// SyncLock
            /// </summary>
            public object? SyncLock { get; set; }
            /// <summary>
            /// WaitEvent
            /// </summary>
            public ManualResetEvent? WaitEvent { get; set; }
        }

        /// <summary>
        /// UploadPartCopyContextWrapper
        /// </summary>
        public class UploadPartCopyContextWrapper
        {
            /// <summary>
            /// Context
            /// </summary>
            public UploadPartCopyContext Context { get; set; }
            /// <summary>
            /// PartNumber
            /// </summary>
            public int PartNumber { get; set; }

            /// <summary>
            /// UploadPartCopyContextWrapper
            /// </summary>
            /// <param name="context"></param>
            /// <param name="partNumber"></param>
            public UploadPartCopyContextWrapper(UploadPartCopyContext context, int partNumber)
            {
                Context = context;
                PartNumber = partNumber;
            }
        }

        /// <summary>
        /// 分片上传。
        /// </summary>
        public static void UploadMultipart()
        {
            var uploadId = InitiateMultipartUpload(Config.AliyunOSS.BucketName, key);
            var partETags = UploadParts(Config.AliyunOSS.BucketName, key, fileToUpload, uploadId, partSize);
            CompleteUploadPart(Config.AliyunOSS.BucketName, key, uploadId, partETags);

            Console.WriteLine("Multipart put object:{0} succeeded", key);
        }

        /// <summary>
        /// 异步分片上传
        /// </summary>
        /// <param name="filePath">文件路径（物理路径，如：d:\\test.zip）</param>
        /// <param name="key">key</param>
        public static void AsyncUploadMultipart(string filePath,string key)
        {
            var uploadId = InitiateMultipartUpload(Config.AliyunOSS.BucketName, key);
            AsyncUploadParts(Config.AliyunOSS.BucketName, key, filePath, uploadId, partSize);
        }

        /// <summary>
        /// 分片拷贝。
        /// </summary>
        public static void UploadMultipartCopy(String targetBucket, String targetObject, String sourceBucket, String sourceObject)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetObject);
            var partETags = UploadPartCopys(targetBucket, targetObject, sourceBucket, sourceObject, uploadId, partSize);
            var completeResult = CompleteUploadPart(targetBucket, targetObject, uploadId, partETags);

            Console.WriteLine(@"Upload multipart copy result : ");
            Console.WriteLine(completeResult.Location);
        }

        /// <summary>
        /// 异步分片拷贝。
        /// </summary>
        public static void AsyncUploadMultipartCopy(String targetBucket, String targetObject, String sourceBucket, String sourceObject)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetObject);
            AsyncUploadPartCopys(targetBucket, targetObject, sourceBucket, sourceObject, uploadId, partSize);
        }
        /// <summary>
        /// 列出所有执行中的Multipart Upload事件
        /// </summary>
        /// <param name="bucketName">目标bucket名称</param>
        public static void ListMultipartUploads(String bucketName)
        {
            var listMultipartUploadsRequest = new ListMultipartUploadsRequest(bucketName);
            var result = client.ListMultipartUploads(listMultipartUploadsRequest);
            Console.WriteLine("Bucket name:" + result.BucketName);
            Console.WriteLine("Key marker:" + result.KeyMarker);
            Console.WriteLine("Delimiter:" + result.Delimiter);
            Console.WriteLine("Prefix:" + result.Prefix);
            Console.WriteLine("UploadIdMarker:" + result.UploadIdMarker);

            foreach (var part in result.MultipartUploads)
            {
                Console.WriteLine(part.ToString());
            }
        }

        private static string InitiateMultipartUpload(String bucketName, String objectName)
        {
            var request = new InitiateMultipartUploadRequest(bucketName, objectName);
            var result = client.InitiateMultipartUpload(request);
            return result.UploadId;
        }

        private static List<PartETag> UploadParts(String bucketName, String objectName, String fileToUpload,
                                                  String uploadId, int partSize)
        {
            var fi = new FileInfo(fileToUpload);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                for (var i = 0; i < partCount; i++)
                {
                    var skipBytes = (long)partSize * i;
                    fs.Seek(skipBytes, 0);
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, objectName, uploadId)
                    {
                        InputStream = fs,
                        PartSize = size,
                        PartNumber = i + 1
                    };
                   
                    var result = client.UploadPart(request);

                    partETags.Add(result.PartETag);
					Console.WriteLine ("finish {0}/{1}", partETags.Count, partCount);
                }
            }
            return partETags;
        }

        private static void AsyncUploadParts(String bucketName, String objectName, String fileToUpload,
            String uploadId, int partSize)
        {
            var fi = new FileInfo(fileToUpload);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var ctx = new UploadPartContext()
            {
                BucketName = bucketName,
                ObjectName = objectName,
                UploadId = uploadId,
                TotalParts = partCount,
                CompletedParts = 0,
                SyncLock = new object(),
                PartETags = new List<PartETag>(),
                WaitEvent = new ManualResetEvent(false)
            };

            for (var i = 0; i < partCount; i++)
            {
                var fs = new FileStream(fileToUpload, FileMode.Open, FileAccess.Read, FileShare.Read);
                var skipBytes = (long)partSize * i;
                fs.Seek(skipBytes, 0);
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request = new UploadPartRequest(bucketName, objectName, uploadId)
                {
                    InputStream = fs,
                    PartSize = size,
                    PartNumber = i + 1
                };
                client.BeginUploadPart(request, UploadPartCallback, new UploadPartContextWrapper(ctx, fs, i + 1));
            }
				
            ctx.WaitEvent.WaitOne();
        }

        private static void UploadPartCallback(IAsyncResult ar)
        {
            var result = client.EndUploadPart(ar);
            var wrappedContext = (UploadPartContextWrapper)ar.AsyncState!;
            wrappedContext.PartStream.Close();

            var ctx = wrappedContext.Context;
            lock (ctx.SyncLock!)
            {
                var partETags = ctx.PartETags;
                partETags!.Add(new PartETag(wrappedContext.PartNumber, result.ETag));
                ctx.CompletedParts++;

				Console.WriteLine ("finish {0}/{1}", ctx.CompletedParts, ctx.TotalParts);
                if (ctx.CompletedParts == ctx.TotalParts)
                {
                    partETags.Sort((e1, e2) => (e1.PartNumber - e2.PartNumber));
                    var completeMultipartUploadRequest = 
                        new CompleteMultipartUploadRequest(ctx.BucketName, ctx.ObjectName, ctx.UploadId);
                    foreach (var partETag in partETags)
                    {
                        completeMultipartUploadRequest.PartETags.Add(partETag);
                    }

                    var completeMultipartUploadResult = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                    Console.WriteLine(@"Async upload multipart result : " + completeMultipartUploadResult.Location);

                    ctx.WaitEvent!.Set();
                }
            }
        }

        private static List<PartETag> UploadPartCopys(String targetBucket, String targetObject, String sourceBucket, String sourceObject,
            String uploadId, int partSize)
        {
            var metadata = client.GetObjectMetadata(sourceBucket, sourceObject);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetObject, sourceBucket, sourceObject, uploadId)
                    {
                        PartSize = size,
                        PartNumber = i + 1,
                        BeginIndex = skipBytes
                    };
                var result = client.UploadPartCopy(request);
                partETags.Add(result.PartETag);
            }

            return partETags;
        }

        private static void AsyncUploadPartCopys(String targetBucket, String targetObject, String sourceBucket, String sourceObject,
            String uploadId, int partSize)
        {
            var metadata = client.GetObjectMetadata(sourceBucket, sourceObject);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var ctx = new UploadPartCopyContext()
            {
                TargetBucket = targetBucket,
                TargetObject = targetObject,
                UploadId = uploadId,
                TotalParts = partCount,
                CompletedParts = 0,
                SyncLock = new object(),
                PartETags = new List<PartETag>(),
                WaitEvent = new ManualResetEvent(false)
            };

            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetObject, sourceBucket, sourceObject, uploadId)
                    {
                        PartSize = size,
                        PartNumber = i + 1,
                        BeginIndex = skipBytes
                    };
                client.BeginUploadPartCopy(request, UploadPartCopyCallback, new UploadPartCopyContextWrapper(ctx, i + 1));
            }

            ctx.WaitEvent.WaitOne();
        }

        private static void UploadPartCopyCallback(IAsyncResult ar)
        {
            var result = client.EndUploadPartCopy(ar);
            var wrappedContext = (UploadPartCopyContextWrapper)ar.AsyncState!;

            var ctx = wrappedContext.Context;
            lock (ctx.SyncLock!)
            {
                var partETags = ctx.PartETags;
                partETags!.Add(new PartETag(wrappedContext.PartNumber, result.ETag));
                ctx.CompletedParts++;

                if (ctx.CompletedParts == ctx.TotalParts)
                {
                    partETags.Sort((e1, e2) => (e1.PartNumber - e2.PartNumber));
                    var completeMultipartUploadRequest =
                        new CompleteMultipartUploadRequest(ctx.TargetBucket, ctx.TargetObject, ctx.UploadId);
                    foreach (var partETag in partETags)
                    {
                        completeMultipartUploadRequest.PartETags.Add(partETag);
                    }

                    var completeMultipartUploadResult = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                    Console.WriteLine(@"Async upload multipart copy result : " + completeMultipartUploadResult.Location);

                    ctx.WaitEvent!.Set();
                }
            }
        }

        private static CompleteMultipartUploadResult CompleteUploadPart(String bucketName, String objectName,
            String uploadId, List<PartETag> partETags)
        {
            var completeMultipartUploadRequest =
                new CompleteMultipartUploadRequest(bucketName, objectName, uploadId);
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }

            return client.CompleteMultipartUpload(completeMultipartUploadRequest);
        }
    }
 }

