# ExpertVideoToolbox
A lightweight, versatile GUI of x264, x265. Nearly full input formats support, .mkv and .mp4 output support. Avs support has been added.

= Mandatory Prerequisites =
* NuGet packages: MediaInfoDotNet and Newtonsoft.Json
* Multiple tools in tools/. (See README.jpg or download one of releases to obtain these tools).
Includes ffmpeg.exe, mkvmerge.exe, qaac.exe (need some .dll files to run without QuickTime), mp4Box.exe(libgpac_static.lib needed), x26x.
Currently,
x265 binary comes from http://msystem.waw.pl/x265/
x264 and ffmpeg binaries come from http://maruko.appinn.me/7mod.html

= Features =
* Easy to bulk convert videos, even with different parameters.
* Full video conversion process (Audio and Video) in one step.
* For expert, easy to modify x26x command lines parameters.
* Low speed mode. Set CPU affinity to one quarter logcial processors. Users do not need to stop the video converting process when playing games or other tasks need CPU.   
* Support .avs script file. Also support audio process (Copy audio & qaac) in avs mode.
