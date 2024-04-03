// See https://aka.ms/new-console-template for more information

using NekoDownloader.Infrastructure.Sources;

var a = new LectorknsSource();

/*var b = await a.GetComic("soy-el-rey-de-los-virus");*/
var c = await a.GetChapter("https://lectorkns.com/sr/solo-subo-de-nivel/capitulo-107/");

/*foreach (var chapter in b.Chapters)
{
    Console.WriteLine(chapter.Title);
    Console.WriteLine(chapter.Number);
    Console.WriteLine(chapter.Link);
    Console.WriteLine();
}*/

foreach (var page in c)
{
    Console.WriteLine(page.Number);
    Console.WriteLine(page.Link);
    Console.WriteLine();
    var bytes = await a.GetPage(page.Link);
    File.WriteAllBytes($"C:\\Users\\{Environment.UserName}\\Downloads\\{page.Number}.jpg", bytes);
}