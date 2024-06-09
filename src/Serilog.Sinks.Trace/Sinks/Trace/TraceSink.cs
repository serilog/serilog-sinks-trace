// Copyright 2013-2016 Serilog Contributors
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
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Trace;

class TraceSink : ILogEventSink
{
    readonly ITextFormatter _textFormatter;

    public TraceSink(ITextFormatter textFormatter)
    {
        _textFormatter = textFormatter;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
        var sr = new StringWriter();
        _textFormatter.Format(logEvent, sr);

        var text = sr.ToString().Trim();

        switch (logEvent.Level)
        {
            case LogEventLevel.Error:
            case LogEventLevel.Fatal:
                System.Diagnostics.Trace.TraceError(text);
                break;
            case LogEventLevel.Warning:
                System.Diagnostics.Trace.TraceWarning(text);
                break;
            case LogEventLevel.Information:
                System.Diagnostics.Trace.TraceInformation(text);
                break;
            default:
                System.Diagnostics.Trace.WriteLine(text);
                break;
        }
    }
}