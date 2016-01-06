﻿// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Amazon.KinesisFirehose;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.Amazon.Kinesis;

namespace Serilog.Sinks.Amazon.Kinesis.Firehose
{
    /// <summary>
    /// Provides KinesisFirehoseSink with configurable options
    /// </summary>
    public class KinesisFirehoseSinkOptions : KinesisSinkOptionsBase
    {
        /// <summary>
        /// The Amazon Kinesis client.
        /// </summary>
        public IAmazonKinesisFirehose KinesisFirehoseClient { get; set; }

        /// <summary>
        /// Will be appended to buffer base filenames.
        /// </summary>
        public override string BufferBaseFilenameAppend { get { return ".firehose"; } }

        /// <summary>
        /// Configures the Amazon Kinesis sink defaults.
        /// </summary>
        protected KinesisFirehoseSinkOptions()
        {
            Period = DefaultPeriod;
            BatchPostingLimit = DefaultBatchPostingLimit;
        }

        /// <summary>
        /// Configures the Amazon Kinesis sink.
        /// </summary>
        /// <param name="kinesisFirehoseClient">The Amazon Kinesis Firehose client.</param>
        /// <param name="streamName">The name of the Kinesis stream.</param>
        /// <param name="shardCount"></param>
        public KinesisFirehoseSinkOptions(IAmazonKinesisFirehose kinesisFirehoseClient, string streamName, int? shardCount = null)
            : this()
        {
            if (kinesisFirehoseClient == null) throw new ArgumentNullException("kinesisFirehoseClient");
            if (streamName == null) throw new ArgumentNullException("streamName");

            KinesisFirehoseClient = kinesisFirehoseClient;
            StreamName = streamName;
        }
    }
}