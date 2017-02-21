# ExpertVideoToolbox
A lightweight, versatile GUI of x264, x265. Nearly full input formats support, .mkv and .mp4 output support. Avs support will be added soon.

= Mandatory Prerequisites =
* NuGet packages: MediaInfoDotNet and Newtonsoft.Json
* Multiple tools in tools/. (See README.jpg or download one of releases to obtain these tools).
Includes ffmpeg.exe, mkvmerge.exe, qaac.exe (need some .dll files to run without QuickTime), mp4Box.exe(libgpac_static.lib needed), unofficial x26x which can feed multiple video formats (original x26x can only read raw YUV files).

= Features =
* Easy to bulk convert videos, even with different parameters.
* Full video conversion process (Audio and Video) in one step.
* For expert, easy to modify x26x command lines parameters.
* Low speed mode. Set CPU affinity to one quarter logcial processors. Users do not need to stop the video converting process when playing games or other tasks need CPU.   

= Develop Plans = 
* Support .avs input. April 2017.