LogPipe
=======
LogPipe is a windows service collecting log data from multiple sources and relaying it to a centralized storage, Elasticsearch. Much similar to the popular Logstash, and just like with Logstash Kibana can be used to read all data collected in Elasticsearch.

#Getting Started
###Installation
LogPipe uses TopShelf (http://topshelf-project.com/) to simplify its installation process.

* To install run: `Consortio.LogPipe.Host.exe install`
* To uninstall run: `Consortio.LogPipe.Host.exe uninstall`

For more information http://docs.topshelf-project.com/en/latest/overview/commandline.html

###Concepts
Important concepts to understand in LogPipe; inputs, pipeline, events, filters and outputs.

####Inputs
Inputs are used to specify a particular type of log, for example a set of rolling files for MyApp.
Currently LogPipe only support file inputs. For each input you must provide a type name and a path.
The type name will later be used to determine what filters to apply and to which outputs to send the logs.
The input path can point to a specific log file or to a directory of log files. Wild cards can be used to only target files matching a given pattern, for example "*.log".

In addition to type name you can also associate tags with an input. Tags can be used just like type name when applying filters and outputs.

LogPipe will watch all defined inputs and create event objects for all log entries it detects.
For a file input a new event object is created for every linebreak.
LogPipe also keeps track of the last processed line number for each file to provide catch up functionallity in case you want to read old log files or if you must stop the LogPipe service for maintenance.
It's also used to pause the input in case the processing of filters and outputs later in the pipeline is slower than the input.

Once an event object is created it's passed down the pipeline to be processed by any matching filters and outputs. 

####Pipeline
A pipeline is created for every input type. It contains all filters and outputs matching that particular input. It will control the flow of how an event object is processed.

####Events
An event object contains the context of a log entry through out the pipeline as the original log entry is being processed.
The event object will in addition to the original log entry contain information about:
* Source - The file path from which the event originated.
* Type - The type name for the input.
* Timestamp - The time for when the log entry was processed, OR if the ExtractTimestampFilter is used the value which is defined within the log entry it self.
* Message - The original log message.
* Tags - A list of tags added by either the input or a filter.
* Fields - A list of fields (key-value pair) extracted by a filter.

####Filters
Filters are used to manipulate the raw message of a log entry. It can replace/transform the original message or it can extract parts of the message to fields or tags. Fields and tags are much easiser to search, filter and drilldown on when browsing your logs in Elasticsearch/Kibana.

All filters can contain a condition element used to determine which events should be processed by the filter:
```
<Conditions>
  <Type>myapp</Type>
  <Expression Match="Field.MyField">.*</Expression>
</Conditions>
```
Type is the input type name which must match and expression is a regex which must match for the filter to be applied. "Expression/@Match" can be the message, source or any field property of the event object and the property value is matched against the regex. If "Expression/@Match" is left out the default is to match on is the message property.
User defined patterns can be used within the regex to simplify the expression. See below for more information on patterns.

LogPipe currently has 4 built-in filters:

* <b>ExtractTimestampFilter</b> - Trys to extract a datetime from the log message. 2013-09-19 12:04:56Z for UTC time, 2013-09-19 12:04:56 for local time. The extracted value will be parsed and saved as UTC in event.Timestamp and in a field called "event_time".
* <b>MatchFilter</b> - Will be applied to matching event objects and can contain actions to be performed. Valid actions are AddField, RemoveField, AddTag, RemoveTag, ExtractFields, Replace and Remove.
* <b>DropFilter</b> - Will drop the event if matched.
* <b>MultiLineFilter</b> - Used to concatenate log entries that stretches over multiple lines. The condition should match the begining (the first line) of each event.

####Outputs
LogPipe only has a single output which stores the events in ElasticSearch. The events will be stored using the same format as Logstash making all data readable by Kibana. 

####Patterns

###Configuration
Configuration can be found in App.config of the Log Pipe Service project.

####Inputs
```
<LogPipe>
  <Input>
    <File Type="iislogs">
      <Path>C:\inetpub\logs\LogFiles\W3SVC1\*.log</Path>
      <Tags>
        <Tag>tag1</Tag>
      </Tags>
    </File>
    <File Type="myapp">
      <Path>C:\myapp\log4net\*.log</Path>
      <Tags>
        <Tag>tag2</Tag>
      </Tags>
    </File>
  </Input>
</LogPipe>
```
      
####Filters
```
<LogPipe>
  <Filter>
    <ExtractTimestampFilter />

    <MultiLineFilter>
      <Conditions>
        <Type>myapp</Type>
        <Expression>^(DEBUG|WARN|ERROR|INFO|FATAL)</Expression>
      </Conditions>
    </MultiLineFilter>

    <DropFilter>
      <Conditions>
        <Type>iislogs</Type>
        <Expression><![CDATA[(^#)|(?i:/assets/)|(?i:/resources/)]]></Expression>
      </Conditions>
    </DropFilter>

    <MatchFilter>
      <Conditions>
        <Type>iislogs</Type>
        <Expression><![CDATA[^%{DATESTAMP:event_time} %{WORD:sitename} %{HOST:server_name} %{IP:server_ip} %{WORD:http_method} %{URIPATH:uri_path} (?:%{DATA:uri_params}|-) %{NUMBER:port} (?:%{USER:username}|-) %{IPORHOST:client_ip} (?:HTTP/%{NUMBER:http_version}) (?:%{DATA:user_agent}|-) (?:%{DATA:cookie}|-) (?:%{URI:referer}|-) %{IPORHOST:host} %{NUMBER:status} %{NUMBER:substatus} %{NUMBER:win32status} %{NUMBER:bytes_sent} %{NUMBER:bytes_received} %{NUMBER:time_taken}]]></Expression>
      </Conditions>
      <Actions>
        <ExtractFields />
      </Actions>
    </MatchFilter>
    
    <MatchFilter>
      <Conditions>
        <Type>iislogs</Type>
        <Expression><![CDATA[ASP.NET_SessionId=%{WORD:session_id};]]></Expression>
      </Conditions>
      <Actions>
        <ExtractFields />
      </Actions>
    </MatchFilter>
  </Filter>
</LogPipe>
```

####Outputs
```
<LogPipe>
  <Output>
    <ElasticSearch>
      <Host>127.0.0.1</Host>
      <Port>9200</Port>
      <IndexNameFormat>\l\o\g\p\i\p\e\-yyyyMM</IndexNameFormat>
    </ElasticSearch>
  </Output>
</LogPipe>
```

####Patterns
```
<LogPipe>
  <Patterns>
    <Pattern Name="USERNAME"><![CDATA[[a-zA-Z0-9_-]+]]></Pattern>
    <Pattern Name="USER"><![CDATA[%{USERNAME}]]></Pattern>
    <Pattern Name="INT"><![CDATA[(?:[+-]?(?:[0-9]+))]]></Pattern>
    <Pattern Name="BASE10NUM"><![CDATA[(?<![0-9.+-])(?>[+-]?(?:(?:[0-9]+(?:\.[0-9]+)?)|(?:\.[0-9]+)))]]></Pattern>
    <Pattern Name="NUMBER"><![CDATA[(?:%{BASE10NUM})]]></Pattern>
    <Pattern Name="BASE16NUM"><![CDATA[(?<![0-9A-Fa-f])(?:[+-]?(?:0x)?(?:[0-9A-Fa-f]+))]]></Pattern>
    <Pattern Name="BASE16FLOAT"><![CDATA[\b(?<![0-9A-Fa-f.])(?:[+-]?(?:0x)?(?:(?:[0-9A-Fa-f]+(?:\.[0-9A-Fa-f]*)?)|(?:\.[0-9A-Fa-f]+)))\b]]></Pattern>

    <Pattern Name="POSINT"><![CDATA[\b(?:[1-9][0-9]*)\b]]></Pattern>
    <Pattern Name="NONNEGINT"><![CDATA[\b(?:[0-9]+)\b]]></Pattern>
    <Pattern Name="WORD"><![CDATA[\b\w+\b]]></Pattern>
    <Pattern Name="NOTSPACE"><![CDATA[\S+]]></Pattern>
    <Pattern Name="SPACE"><![CDATA[\s*]]></Pattern>
    <Pattern Name="DATA"><![CDATA[.*?]]></Pattern>
    <Pattern Name="GREEDYDATA"><![CDATA[.*]]></Pattern>
    <Pattern Name="QUOTEDSTRING"><![CDATA[(?>(?<!\\)(?>"(?>\\.|[^\\"]+)+"|""|(?>'(?>\\.|[^\\']+)+')|''|(?>`(?>\\.|[^\\`]+)+`)|``))]]></Pattern>
    <Pattern Name="UUID"><![CDATA[[A-Fa-f0-9]{8}-(?:[A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}]]></Pattern>

    <!-- Networking -->
    <Pattern Name="MAC"><![CDATA[(?:%{CISCOMAC}|%{WINDOWSMAC}|%{COMMONMAC})]]></Pattern>
    <Pattern Name="CISCOMAC"><![CDATA[(?:(?:[A-Fa-f0-9]{4}\.){2}[A-Fa-f0-9]{4})]]></Pattern>
    <Pattern Name="WINDOWSMAC"><![CDATA[(?:(?:[A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2})]]></Pattern>
    <Pattern Name="COMMONMAC"><![CDATA[(?:(?:[A-Fa-f0-9]{2}:){5}[A-Fa-f0-9]{2})]]></Pattern>
    <Pattern Name="IP"><![CDATA[(?<![0-9])(?:(?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2})[.](?:25[0-5]|2[0-4][0-9]|[0-1]?[0-9]{1,2}))(?![0-9])]]></Pattern>
    <Pattern Name="HOSTNAME"><![CDATA[\b(?:[0-9A-Za-z][0-9A-Za-z-]{0,62})(?:\.(?:[0-9A-Za-z][0-9A-Za-z-]{0,62}))*(\.?|\b)]]></Pattern>
    <Pattern Name="HOST"><![CDATA[%{HOSTNAME}]]></Pattern>
    <Pattern Name="IPORHOST"><![CDATA[(?:%{HOSTNAME}|%{IP})]]></Pattern>
    <Pattern Name="HOSTPORT"><![CDATA[(?:%{IPORHOST=~/\./}:%{POSINT})]]></Pattern>

    <!-- Paths -->
    <Pattern Name="PATH"><![CDATA[(?:%{UNIXPATH}|%{WINPATH})]]></Pattern>
    <Pattern Name="UNIXPATH"><![CDATA[(?>/(?>[\w_%!$@:.,-]+|\\.)*)+]]></Pattern>
    <Pattern Name="LINUXTTY"><![CDATA[(?>/dev/pts/%{NONNEGINT})]]></Pattern>
    <Pattern Name="BSDTTY"><![CDATA[(?>/dev/tty[pq][a-z0-9])]]></Pattern>
    <Pattern Name="TTY"><![CDATA[(?:%{BSDTTY}|%{LINUXTTY})]]></Pattern>
    <Pattern Name="WINPATH"><![CDATA[(?>[A-Za-z]+:|\\)(?:\\[^\\?*]*)+]]></Pattern>
    <Pattern Name="URIPROTO"><![CDATA[[A-Za-z]+(\+[A-Za-z+]+)?]]></Pattern>
    <Pattern Name="URIHOST"><![CDATA[%{IPORHOST}(?::%{POSINT:port})?]]></Pattern>
    <!-- uripath comes loosely from RFC1738, but mostly from what Firefox doesn't turn into %XX -->
    <Pattern Name="URIPATH"><![CDATA[(?:/[A-Za-z0-9$.+!*'(){},~:;=#%_-]*)+]]></Pattern>
    <Pattern Name="URIPARAM"><![CDATA[\?[A-Za-z0-9$.+!*'|(){},~#%&/=:;_?-\[\]]*]]></Pattern>
    <Pattern Name="URIPATHPARAM"><![CDATA[%{URIPATH}(?:%{URIPARAM})?]]></Pattern>
    <Pattern Name="URI"><![CDATA[%{URIPROTO}://(?:%{USER}(?::[^@]*)?@)?(?:%{URIHOST})?(?:%{URIPATHPARAM})?]]></Pattern>

    <!-- Months: January, Feb, 3, 03, 12, December -->
    <Pattern Name="MONTH"><![CDATA[\b(?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?)\b]]></Pattern>
    <Pattern Name="MONTHNUM"><![CDATA[(?:0?[1-9]|1[0-2])]]></Pattern>
    <Pattern Name="MONTHDAY"><![CDATA[(?:(?:0[1-9])|(?:[12][0-9])|(?:3[01])|[1-9])]]></Pattern>

    <!-- Days: Monday, Tue, Thu, etc... -->
    <Pattern Name="DAY"><![CDATA[(?:Mon(?:day)?|Tue(?:sday)?|Wed(?:nesday)?|Thu(?:rsday)?|Fri(?:day)?|Sat(?:urday)?|Sun(?:day)?)]]></Pattern>
    <Pattern Name="YEAR"><![CDATA[(?>\d\d){1,2}]]></Pattern>
    <Pattern Name="HOUR"><![CDATA[(?:2[0123]|[01][0-9])]]></Pattern>
    <Pattern Name="MINUTE"><![CDATA[(?:[0-5][0-9])]]></Pattern>
    <!-- '60' is a leap second in most time standards and thus is valid. -->
    <Pattern Name="SECOND"><![CDATA[(?:(?:[0-5][0-9]|60)(?:[.,][0-9]+)?)]]></Pattern>
    <Pattern Name="TIME"><![CDATA[(?!<[0-9])%{HOUR}:%{MINUTE}(?::%{SECOND})(?![0-9])]]></Pattern>
    <!-- datestamp is YYYY/MM/DD-HH:MM:SS.UUUU (or something like it) -->
    <Pattern Name="DATE_US"><![CDATA[%{MONTHNUM}[/-]%{MONTHDAY}[/-]%{YEAR}]]></Pattern>
    <Pattern Name="DATE_EU"><![CDATA[%{YEAR}[/-]%{MONTHNUM}[/-]%{MONTHDAY}]]></Pattern>
    <Pattern Name="ISO8601_TIMEZONE"><![CDATA[(?:Z|[+-]%{HOUR}(?::?%{MINUTE}))]]></Pattern>
    <Pattern Name="ISO8601_SECOND"><![CDATA[(?:%{SECOND}|60)]]></Pattern>
    <Pattern Name="TIMESTAMP_ISO8601"><![CDATA[%{YEAR}-%{MONTHNUM}-%{MONTHDAY}[T ]%{HOUR}:?%{MINUTE}(?::?%{SECOND})?%{ISO8601_TIMEZONE}?]]></Pattern>
    <Pattern Name="DATE"><![CDATA[%{DATE_US}|%{DATE_EU}]]></Pattern>
    <Pattern Name="DATESTAMP"><![CDATA[%{DATE}[- ]%{TIME}]]></Pattern>
    <Pattern Name="TZ"><![CDATA[(?:[PMCE][SD]T)]]></Pattern>
    <Pattern Name="DATESTAMP_RFC822"><![CDATA[%{DAY} %{MONTH} %{MONTHDAY} %{YEAR} %{TIME} %{TZ}]]></Pattern>
    <Pattern Name="DATESTAMP_OTHER"><![CDATA[%{DAY} %{MONTH} %{MONTHDAY} %{TIME} %{TZ} %{YEAR}]]></Pattern>

    <!-- Syslog Dates: Month Day HH:MM:SS -->
    <Pattern Name="SYSLOGTIMESTAMP"><![CDATA[%{MONTH} +%{MONTHDAY} %{TIME}]]></Pattern>
    <Pattern Name="PROG"><![CDATA[(?:[\w._/%-]+)]]></Pattern>
    <Pattern Name="SYSLOGPROG"><![CDATA[%{PROG:program}(?:\[%{POSINT:pid}\])?]]></Pattern>
    <Pattern Name="SYSLOGHOST"><![CDATA[%{IPORHOST}]]></Pattern>
    <Pattern Name="SYSLOGFACILITY"><![CDATA[<%{NONNEGINT:facility}.%{NONNEGINT:priority}>]]></Pattern>
    <Pattern Name="HTTPDATE"><![CDATA[%{MONTHDAY}/%{MONTH}/%{YEAR}:%{TIME} %{INT}]]></Pattern>

    <!-- Shortcuts -->
    <Pattern Name="QS"><![CDATA[%{QUOTEDSTRING}]]></Pattern>

    <!-- Log formats -->
    <Pattern Name="SYSLOGBASE"><![CDATA[%{SYSLOGTIMESTAMP:timestamp} (?:%{SYSLOGFACILITY} )?%{SYSLOGHOST:logsource} %{SYSLOGPROG}:]]></Pattern>
    <Pattern Name="COMBINEDAPACHELOG"><![CDATA[%{IPORHOST:clientip} %{USER:ident} %{USER:auth} \[%{HTTPDATE:timestamp}\] "(?:%{WORD:verb} %{NOTSPACE:request}(?: HTTP/%{NUMBER:httpversion})?|-)" %{NUMBER:response} (?:%{NUMBER:bytes}|-) %{QS:referrer} %{QS:agent}]]></Pattern>

    <!-- Log Levels -->
    <Pattern Name="LOGLEVEL"><![CDATA[([T|t]race|TRACE|[D|d]ebug|DEBUG|[N|n]otice|NOTICE|[I|i]nfo|INFO|[W|w]arn?(?:ing)?|WARN?(?:ING)?|[E|e]rr?(?:or)?|ERR?(?:OR)?|[C|c]rit?(?:ical)?|CRIT?(?:ICAL)?|[F|f]atal|FATAL|[S|s]evere|SEVERE)]]></Pattern>
  </Patterns>
</LogPipe>
  ```
  
