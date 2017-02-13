﻿// The MIT License (MIT)
//
// CoreTweet - A .NET Twitter Library supporting Twitter API 1.1
// Copyright (c) 2013-2016 CoreTweet Development Team
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#if ASYNC
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CoreTweet.Core;

namespace CoreTweet.Rest
{
    partial class Media
    {
        //POST methods

        internal Task<AsyncResponse> AccessUploadApiAsync(IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken, IProgress<UploadProgressInfo> progress)
        {
            var options = Tokens.ConnectionOptions ?? ConnectionOptions.Default;
            return this.Tokens.SendRequestAsyncImpl(MethodType.Post, InternalUtils.GetUrl(options, options.UploadUrl, true, "media/upload.json"), parameters, cancellationToken, progress);
        }

        private Task<MediaUploadResult> UploadAsyncImpl(IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken, IProgress<UploadProgressInfo> progress = null)
        {
            return this.AccessUploadApiAsync(parameters, cancellationToken, progress)
                .ReadResponse(s => CoreBase.Convert<MediaUploadResult>(s), cancellationToken);
        }

        #region UploadAsync with progress parameter
        /// <summary>
        /// <para>Upload media (images) to Twitter for use in a Tweet or Twitter-hosted Card.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>Stream</c> media (any one is required)</para>
        /// <para>- <c>IEnumerable&lt;byte&gt;</c> media (any one is required)</para>
        /// <para>- <c>FileInfo</c> media (any one is required)</para>
        /// <para>- <c>string</c> media_data (any one is required)</para>
        /// <para>- <c>IEnumerable&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            return this.UploadAsyncImpl(parameters, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Upload media (images) to Twitter for use in a Tweet or Twitter-hosted Card.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>Stream</c> media (any one is required)</para>
        /// <para>- <c>IEnumerable&lt;byte&gt;</c> media (any one is required)</para>
        /// <para>- <c>FileInfo</c> media (any one is required)</para>
        /// <para>- <c>string</c> media_data (any one is required)</para>
        /// <para>- <c>IEnumerable&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadAsync(object parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            return this.UploadAsyncImpl(InternalUtils.ResolveObject(parameters), cancellationToken, progress);
        }

        /// <summary>
        /// <para>Upload media (images) to Twitter for use in a Tweet or Twitter-hosted Card.</para>
        /// </summary>
        /// <param name="media">any one is required.</param>
        /// <param name="additional_owners">optional.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadAsync(Stream media, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            if (media == null) throw new ArgumentNullException(nameof(media));
            parameters.Add("media", media);
            if (additional_owners != null) parameters.Add("additional_owners", additional_owners);
            return this.UploadAsyncImpl(parameters, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Upload media (images) to Twitter for use in a Tweet or Twitter-hosted Card.</para>
        /// </summary>
        /// <param name="media">any one is required.</param>
        /// <param name="additional_owners">optional.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadAsync(IEnumerable<byte> media, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            if (media == null) throw new ArgumentNullException(nameof(media));
            parameters.Add("media", media);
            if (additional_owners != null) parameters.Add("additional_owners", additional_owners);
            return this.UploadAsyncImpl(parameters, cancellationToken, progress);
        }

#if FILEINFO
        /// <summary>
        /// <para>Upload media (images) to Twitter for use in a Tweet or Twitter-hosted Card.</para>
        /// </summary>
        /// <param name="media">any one is required.</param>
        /// <param name="additional_owners">optional.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadAsync(FileInfo media, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            if (media == null) throw new ArgumentNullException(nameof(media));
            parameters.Add("media", media);
            if (additional_owners != null) parameters.Add("additional_owners", additional_owners);
            return this.UploadAsyncImpl(parameters, cancellationToken, progress);
        }
#endif
        #endregion

        private Task<AsyncResponse> CommandAsync(string command, IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken, IProgress<UploadProgressInfo> progress = null)
        {
            return this.AccessUploadApiAsync(parameters.EndWith(new KeyValuePair<string, object>("command", command)), cancellationToken, progress);
        }

        private Task<T> CommandAsync<T>(string command, IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken)
        {
            return this.CommandAsync(command, parameters, cancellationToken)
                .ReadResponse(s => CoreBase.Convert<T>(s), cancellationToken);
        }

        private Task<UploadInitCommandResult> UploadInitCommandAsyncImpl(IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken)
        {
            return this.CommandAsync<UploadInitCommandResult>("INIT", parameters, cancellationToken);
        }

        private Task UploadAppendCommandAsyncImpl(IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken, IProgress<UploadProgressInfo> progress = null)
        {
            return this.CommandAsync("APPEND", parameters, cancellationToken, progress)
                .Done(res => res.Dispose(), CancellationToken.None);
        }

        #region UploadAppendCommand with progress parameter
        /// <summary>
        /// <para>Upload(s) of chunked data.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> media_id (required)</para>
        /// <para>- <c>int</c> segment_index (required)</para>
        /// <para>- <c>Stream</c> media (any one is required)</para>
        /// <para>- <c>IEnumerable&lt;byte&gt;</c> media (any one is required)</para>
        /// <para>- <c>FileInfo</c> media (any one is required)</para>
        /// <para>- <c>string</c> media_data (any one is required)</para>
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task UploadAppendCommandAsync(IDictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            return this.UploadAppendCommandAsyncImpl(parameters, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Upload(s) of chunked data.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>long</c> media_id (required)</para>
        /// <para>- <c>int</c> segment_index (required)</para>
        /// <para>- <c>Stream</c> media (any one is required)</para>
        /// <para>- <c>IEnumerable&lt;byte&gt;</c> media (any one is required)</para>
        /// <para>- <c>FileInfo</c> media (any one is required)</para>
        /// <para>- <c>string</c> media_data (any one is required)</para>
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task UploadAppendCommandAsync(object parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            return this.UploadAppendCommandAsyncImpl(InternalUtils.ResolveObject(parameters), cancellationToken, progress);
        }

        /// <summary>
        /// <para>Upload(s) of chunked data.</para>
        /// </summary>
        /// <param name="media_id">required.</param>
        /// <param name="segment_index">required.</param>
        /// <param name="media">any one is required.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task UploadAppendCommandAsync(long media_id, int segment_index, Stream media, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("media_id", media_id);
            parameters.Add("segment_index", segment_index);
            if (media == null) throw new ArgumentNullException(nameof(media));
            parameters.Add("media", media);
            return this.UploadAppendCommandAsyncImpl(parameters, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Upload(s) of chunked data.</para>
        /// </summary>
        /// <param name="media_id">required.</param>
        /// <param name="segment_index">required.</param>
        /// <param name="media">any one is required.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task UploadAppendCommandAsync(long media_id, int segment_index, IEnumerable<byte> media, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("media_id", media_id);
            parameters.Add("segment_index", segment_index);
            if (media == null) throw new ArgumentNullException(nameof(media));
            parameters.Add("media", media);
            return this.UploadAppendCommandAsyncImpl(parameters, cancellationToken, progress);
        }

#if FILEINFO
        /// <summary>
        /// <para>Upload(s) of chunked data.</para>
        /// </summary>
        /// <param name="media_id">required.</param>
        /// <param name="segment_index">required.</param>
        /// <param name="media">any one is required.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task UploadAppendCommandAsync(long media_id, int segment_index, FileInfo media, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("media_id", media_id);
            parameters.Add("segment_index", segment_index);
            if (media == null) throw new ArgumentNullException(nameof(media));
            parameters.Add("media", media);
            return this.UploadAppendCommandAsyncImpl(parameters, cancellationToken, progress);
        }
#endif
        #endregion

        private Task<UploadFinalizeCommandResult> UploadFinalizeCommandAsyncImpl(IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken)
        {
            return this.CommandAsync<UploadFinalizeCommandResult>("FINALIZE", parameters, cancellationToken);
        }

        private Task<UploadFinalizeCommandResult> UploadStatusCommandAsyncImpl(IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellationToken)
        {
            var options = Tokens.ConnectionOptions ?? ConnectionOptions.Default;
            return this.Tokens.SendRequestAsyncImpl(MethodType.Get, InternalUtils.GetUrl(options, options.UploadUrl, true, "media/upload.json"),
                parameters.EndWith(new KeyValuePair<string, object>("command", "STATUS")), cancellationToken)
                .ReadResponse(s => CoreBase.Convert<UploadFinalizeCommandResult>(s), cancellationToken);
        }

        private Task<MediaUploadResult> UploadChunkedAsyncImpl(Stream media, long totalBytes, UploadMediaType mediaType, IEnumerable<KeyValuePair<string, object>> parameters, int retryCount, int delay, CancellationToken cancellationToken, IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadInitCommandAsyncImpl(
                parameters.EndWith(
                    new KeyValuePair<string, object>("total_bytes", totalBytes),
                    new KeyValuePair<string, object>("media_type", mediaType)
                ), cancellationToken)
                .Done(result =>
                {
                    const int maxChunkSize = 5 * 1000 * 1000;
                    var estSegments = (int)((totalBytes + maxChunkSize - 1) / maxChunkSize);
                    var tasks = new List<Task>(estSegments);
                    var sem = new Semaphore(2, 2);
                    var remainingBytes = totalBytes;

                    List<UploadProgressInfo> reports = null;
                    Action<int, UploadProgressInfo> uploadReport = null;
                    Action<UploadFinalizeCommandResult> statusReport = null;
                    var lastReport = new UploadChunkedProgressInfo(UploadChunkedProgressStage.SendingContent, 0, totalBytes, 0);
                    if (progress != null)
                    {
                        reports = new List<UploadProgressInfo>(estSegments);
                        uploadReport = (segmentIndex, info) =>
                        {
                            // Lock not to conflict with Add.
                            lock (reports)
                                reports[segmentIndex] = info;
                            long bytesSent = 0;
                            long? totalBytesToSend = remainingBytes;
                            // Don't use foreach to avoid InvalidOperationException.
                            for (var i = 0; i < reports.Count; i++)
                            {
                                var x = reports[i];
                                bytesSent += x.BytesSent;
                                totalBytesToSend += x.TotalBytesToSend;
                            }
                            if (totalBytesToSend.HasValue)
                                lastReport.TotalBytesToSend = totalBytesToSend.Value;
                            lastReport.BytesSent = bytesSent;
                            progress.Report(lastReport);
                        };
                        statusReport = x =>
                        {
                            if (x.ProcessingInfo == null) return;
                            switch (x.ProcessingInfo.State)
                            {
                                case "pending":
                                    lastReport.Stage = UploadChunkedProgressStage.Pending;
                                    break;
                                case "in_progress":
                                    lastReport.Stage = UploadChunkedProgressStage.InProgress;
                                    break;
                            }
                            var progressPercent = x.ProcessingInfo.ProgressPercent;
                            if (progressPercent.HasValue)
                                lastReport.ProcessingProgressPercent = progressPercent.Value;
                            progress.Report(lastReport);
                        };
                    }

                    for (var segmentIndex = 0; remainingBytes > 0; segmentIndex++)
                    {
                        sem.WaitOne();
                        if (tasks.Any(x => x.IsFaulted)) break;

                        var chunkSize = (int)Math.Min(remainingBytes, maxChunkSize);
                        var chunk = new byte[chunkSize];
                        var readCount = media.Read(chunk, 0, chunkSize);
                        if (readCount == 0) break;
                        remainingBytes -= readCount;
                        if (reports != null)
                        {
                            lock (reports)
                                reports.Add(new UploadProgressInfo(0, readCount));
                        }
                        tasks.Add(
                            this.AppendCore(result.MediaId, segmentIndex, new ArraySegment<byte>(chunk, 0, readCount), retryCount, delay, cancellationToken, uploadReport)
                                .ContinueWith(t =>
                                {
                                    sem.Release();
                                    return t;
                                })
                                .Unwrap()
                        );
                    }

                    return Task.WhenAll(tasks)
                        .Done(() => this.UploadFinalizeCommandAsync(result.MediaId, cancellationToken), cancellationToken)
                        .Unwrap()
                        .Done(x =>
                        {
                            statusReport?.Invoke(x);

                            return x.ProcessingInfo?.CheckAfterSecs != null
                                ? this.WaitForProcessing(x.MediaId, cancellationToken, statusReport)
                                : Task.FromResult(x);
                        }, cancellationToken)
                        .Unwrap()
                        .Done(x => x as MediaUploadResult, cancellationToken);
                }, cancellationToken, TaskContinuationOptions.LongRunning)
                .Unwrap();
        }

        private Task AppendCore(long mediaId, int segmentIndex, ArraySegment<byte> media, int retryCount, int delay, CancellationToken cancellationToken, Action<int, UploadProgressInfo> report)
        {
            var task = this.UploadAppendCommandAsyncImpl(
                new Dictionary<string, object>
                {
                    { "media_id", mediaId },
                    { "segment_index", segmentIndex },
                    { "media", media }
                },
                cancellationToken,
                report == null ? null : new SimpleProgress<UploadProgressInfo>(x => report(segmentIndex, x))
            ).ContinueWith(t => t.Exception != null && retryCount > 0
                // Retry
                ? Task.Delay(delay, cancellationToken).ContinueWith(_ =>
                    this.AppendCore(mediaId, segmentIndex, media, retryCount - 1, delay, cancellationToken, report))
                    .Unwrap()
                : t
            ).Unwrap();

            if (report != null)
                task = task.Done(() => report(segmentIndex, new UploadProgressInfo(media.Count, media.Count)), cancellationToken);

            return task;
        }

        private Task<UploadFinalizeCommandResult> WaitForProcessing(long mediaId, CancellationToken cancellationToken, Action<UploadFinalizeCommandResult> report)
        {
            return this.UploadStatusCommandAsync(mediaId, cancellationToken)
                .ContinueWith(t =>
                {
                    if (t.Status == TaskStatus.RanToCompletion)
                    {
                        var res = t.Result;

                        if (res.ProcessingInfo?.State == "failed")
                            throw new MediaProcessingException(res);

                        report?.Invoke(res);

                        if (res.ProcessingInfo?.CheckAfterSecs != null)
                        {
                            return Task.Delay(res.ProcessingInfo.CheckAfterSecs.Value * 1000, cancellationToken)
                                .Done(() => this.WaitForProcessing(mediaId, cancellationToken, report), cancellationToken)
                                .Unwrap();
                        }

                        return Task.FromResult(res);
                    }

                    if (t.Exception != null)
                    {
                        var ex = t.Exception.InnerException;
                        // Be sure that ex is not caused by a bug
                        if (!(ex is TwitterException || ex is NullReferenceException || ex is ArgumentException))
                        {
                            // Retry
                            return Task.Delay(5000, cancellationToken)
                                .Done(() => this.WaitForProcessing(mediaId, cancellationToken, report), cancellationToken)
                                .Unwrap();
                        }
                    }

                    return t;
                }, cancellationToken)
                .Unwrap();
        }

        #region UploadChunkedAsync
        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, long totalBytes, UploadMediaType mediaType, params Expression<Func<string, object>>[] parameters)
        {
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, InternalUtils.ExpressionsToDictionary(parameters), 0, 0, CancellationToken.None);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, UploadMediaType mediaType, params Expression<Func<string, object>>[] parameters)
        {
            return this.UploadChunkedAsync(media, media.Length, mediaType, parameters);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, long totalBytes, UploadMediaType mediaType, IDictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, parameters, 0, 0, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, long totalBytes, UploadMediaType mediaType, object parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, InternalUtils.ResolveObject(parameters), 0, 0, cancellationToken, progress);
        }

        /// <summary>
        /// Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="media_category">A string enum value which identifies a media usecase.</param>
        /// <param name="additional_owners">A comma-separated string of user IDs to set as additional owners who are allowed to use the returned media_id in Tweets or Cards.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, long totalBytes, UploadMediaType mediaType, string media_category = null, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            if (media_category != null) parameters.Add(nameof(media_category), media_category);
            if (additional_owners != null) parameters.Add(nameof(additional_owners), additional_owners);
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, parameters, 0, 0, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, UploadMediaType mediaType, IDictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsync(media, media.Length, mediaType, parameters, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, UploadMediaType mediaType, object parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsync(media, media.Length, mediaType, parameters, cancellationToken, progress);
        }

        /// <summary>
        /// Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="media_category">A string enum value which identifies a media usecase.</param>
        /// <param name="additional_owners">A comma-separated string of user IDs to set as additional owners who are allowed to use the returned media_id in Tweets or Cards.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadChunkedAsync(Stream media, UploadMediaType mediaType, string media_category = null, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsync(media, media.Length, mediaType, media_category, additional_owners, cancellationToken, progress);
        }
        #endregion

        #region UploadChunkedWithRetryAsync
        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, long totalBytes, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, params Expression<Func<string, object>>[] parameters)
        {
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, InternalUtils.ExpressionsToDictionary(parameters), retryCount, retryDelayInMilliseconds, CancellationToken.None);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, params Expression<Func<string, object>>[] parameters)
        {
            return this.UploadChunkedWithRetryAsync(media, media.Length, mediaType, retryCount, retryDelayInMilliseconds, parameters);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, long totalBytes, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, IDictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, parameters, retryCount, retryDelayInMilliseconds, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, long totalBytes, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, object parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, InternalUtils.ResolveObject(parameters), retryCount, retryDelayInMilliseconds, cancellationToken, progress);
        }

        /// <summary>
        /// Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="totalBytes">The size of the media being uploaded in bytes.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="media_category">A string enum value which identifies a media usecase.</param>
        /// <param name="additional_owners">A comma-separated string of user IDs to set as additional owners who are allowed to use the returned media_id in Tweets or Cards.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, long totalBytes, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, string media_category = null, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            var parameters = new Dictionary<string, object>();
            if (media_category != null) parameters.Add(nameof(media_category), media_category);
            if (additional_owners != null) parameters.Add(nameof(additional_owners), additional_owners);
            return this.UploadChunkedAsyncImpl(media, totalBytes, mediaType, parameters,retryCount, retryDelayInMilliseconds, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, IDictionary<string, object> parameters, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedWithRetryAsync(media, media.Length, mediaType, retryCount, retryDelayInMilliseconds, parameters, cancellationToken, progress);
        }

        /// <summary>
        /// <para>Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.</para>
        /// <para>Available parameters:</para>
        /// <para>- <c>string</c> media_category (optional)</para>
        /// <para>- <c>IEnumerbale&lt;long&gt;</c> additional_owners (optional)</para>
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// <para>The task object representing the asynchronous operation.</para>
        /// <para>The Result property on the task object returns the result for the uploaded media.</para>
        /// </returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, UploadMediaType mediaType, object parameters, int retryCount, int retryDelayInMilliseconds, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedWithRetryAsync(media, media.Length, mediaType, retryCount, retryDelayInMilliseconds, parameters, cancellationToken, progress);
        }

        /// <summary>
        /// Uploads videos or chunked images to Twitter for use in a Tweet or Twitter-hosted Card as an asynchronous operation.
        /// </summary>
        /// <param name="media">The raw binary file content being uploaded.</param>
        /// <param name="mediaType">The type of the media being uploaded.</param>
        /// <param name="media_category">A string enum value which identifies a media usecase.</param>
        /// <param name="additional_owners">A comma-separated string of user IDs to set as additional owners who are allowed to use the returned media_id in Tweets or Cards.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result for the uploaded media.</returns>
        public Task<MediaUploadResult> UploadChunkedWithRetryAsync(Stream media, UploadMediaType mediaType, int retryCount, int retryDelayInMilliseconds, string media_category = null, IEnumerable<long> additional_owners = null, CancellationToken cancellationToken = default(CancellationToken), IProgress<UploadChunkedProgressInfo> progress = null)
        {
            return this.UploadChunkedWithRetryAsync(media, media.Length, mediaType, retryCount, retryDelayInMilliseconds, media_category, additional_owners, cancellationToken, progress);
        }
        #endregion
    }
}
#endif
