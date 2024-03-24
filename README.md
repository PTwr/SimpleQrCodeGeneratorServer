# WHY?!

Because injecting QR code by URL into Excel is easiest way, and using online services throws you into quagmire of GDPR and corpo security, while existing solutions were harder to use than "just run the .exe, mate!" :)


# How to use

Just snatch release package, unzip, and run the .exe, it will start at port 5000.

You can change port number in `appsettings.json`

When done generating QR Codes just close its console window.

## Why the hell does exe weight 86 megabytes?!

Its a stand-alone release, contains all the necessary stuff so ya won't have to install anything. Fancy, eh?

Instructing office workers on how to install specific version of .NET Framework is not fun.

# Requirements

If you can run Excel, you can run this :)

However, depending on how strict security is on your corpo provided laptop, you might not be able to host HTTP server. 
As counter arguments to get it whitelisted, or hosted on company server, you can just say it skips all the GDPR bullshit by keeping everything local.

# Usage

Examples
- `http://localhost:5000/?text=you%20can%20rely%20on%20default%20values`
- `http://localhost:5000/?text=Or%20Provide%20Everything&color=pink&backgroundColor=LightCoral&blockSize=10&ECC=H`

Everything is optional, and has good default values, but if you don't provide `text` ya gonna have nothing in QR Code :)

Parameters:
- text: the stuff ya gonna get into QR Code, remember to `EncodeURL` or ya gonna have bad time
- color: primary color (filled squares), can be hex or named color. Default: black
- backgroundColor: secondary color (background), can be hex or named color. Default: white
- blockSize: pixel size of each square, its an integer number. Default: 10
- ECC: error correction level, possible values are L (~7% ECC), M (~15% ECC), Q (~25% ECC), H (~30% ECC). Only first letter is checked so you can send words to make code more readable. Default: M (~15% ECC)

List of supported "named colors": https://learn.microsoft.com/en-us/dotnet/api/system.drawing.knowncolor?view=net-8.0

# How to consume from VBA

Watch this tutorial: https://www.youtube.com/watch?v=9ETS2qK_jYc (thats how I learned how to do it :D)
Or read the docs: https://learn.microsoft.com/en-us/office/vba/api/excel.shapes.addpicture

Important bit is that you gotta pass `false` as second parameter and `true` as third parameter to make picture "permanent". 
Remember when your school presentation failed because pictures were linked from path/web instead of being embedeed into single file? Yeah, thats the issue you gotta prevent here with those bool flags.

Heres basic VBA snippet to get shit up and running. It will take text from A1 and slap QR Code into your selection.

```
Sub AddPicture()

 Dim data As String
 data = ActiveSheet.Cells(1, "A").Text

'Added in 2013, for older versions see https://stackoverflow.com/questions/218181/how-can-i-url-encode-a-string-in-excel-vba
 data = WorksheetFunction.EncodeURL(data) 

 Dim Url As String
 Dim x As Integer, y As Integer, w As Integer, h As Integer
 Url = "http://localhost:5000/?text=" + data + "&color&color=black&backgroundColor=white&blockSize=20&ECC=H"
 x = Selection.Left
 y = Selection.Top
 w = Selection.Width
 h = Selection.Height
 
 ActiveSheet.Shapes.AddPicture Url, msoFalse, msoTrue, x, y, w, h
End Sub
```

# Licensing

Its MIT, which basially means you can do anything (legal) without being scared of what viral nature of GPL would do to your corpo secrets :)

Oh, and I keep the authorship and stuff. But lets be real, writing this Readme took waay more time than writing the program! Its like 10 lines of copy&pasted code, there's nothing worth of stealing here :D
