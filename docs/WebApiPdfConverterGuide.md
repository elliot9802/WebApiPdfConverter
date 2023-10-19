# WebApiPdfConverter API-guide

## Inneh�llsf�rteckning
1. [�versikt](#�versikt)
2. [Installation](#installation)
   - [F�ruts�ttningar](#f�ruts�ttningar)
   - [Steg-f�r-steg installation](#steg-f�r-steg-installation)
3. [Anv�ndning](#anv�ndning)
   - [Konvertera URL till PDF](#konvertera-url-till-pdf)
   - [Exempel](#exempel)
4. [Felhantering](#felhantering)

## �versikt
WebApiPdfConverter �r en webb-API som l�ter anv�ndare konvertera webbplatser till PDF-filer. 
API:n erbjuder enkla slutpunkter f�r att skicka en webbadress (URL) eller en HTML-fil och f� tillbaka en PDF-representation.

## Installation
F�r en framg�ngsrik installation av WebApiPdfConverter, 
se till att uppfylla de angivna f�ruts�ttningarna och f�lj sedan installationsstegen noggrant.

### F�ruts�ttningar
- .NET Core installerat p� din dator.
- En IDE som st�der C# och .NET Core, t.ex. Visual Studio.

### Steg-f�r-steg installation
1. Klona eller ladda ner koden fr�n git-repot.
2. �ppna l�sningen i din IDE.
3. �terst�ll alla NuGet-paket om det beh�vs.
4. Bygg projektet f�r att s�kerst�lla att det inte finns n�gra kompileringsfel.
5. K�r API:t antingen genom att trycka p� "Start" i din IDE eller genom att anv�nda terminalen och skriva `dotnet run`.

## Anv�ndning

### Konvertera URL till PDF
F�r att konvertera en webbadress (URL) till PDF, anv�nd f�ljande slutpunkt:

- **Metod:** POST
- **Slutpunkt:** `/api/pdf/convertUrl`

**Payload:**
```json
{
    "Url": "<din_webbplats_url>"
}
```
**Svar:**
En PDF-fil som representerar den angivna webbadressens inneh�ll.

### Konvertera HTML-fil till PDF
F�r att konvertera en uppladdad HTML-fil till PDF, anv�nd f�ljande slutpunkt:

- **Metod:** POST
- **Slutpunkt:** `/api/pdf/convertHtmlFile`

**Payload:** 
En `multipart/form-data`-beg�ran med en filparameter `htmlFile` som inneh�ller HTML-filen som ska konverteras.
**Svar:** En PDF-fil som representerar inneh�llet i den uppladdade HTML-filen.

### Exempel

**Konvertera URL till PDF:**
```bash
curl -X POST 
     -H "Content-Type: application/json" 
     -d '{"Url": "http://example.com"}' 
     https://ApiUrl/api/pdf/convertUrl
```

### Exempel
Anv�nd f�ljande curl-kommando f�r att testa API:t:
```bash
curl -X POST "https://ApiUrl/api/pdf/convertUrl" \
     -H "Content-Type: application/json" \
     -d '{"Url": "https://www.example.com"}' \
     --output converted.pdf
```
Detta kommando kommer att skicka en f�rfr�gan till API:t f�r att konvertera "https://www.example.com" till PDF och spara den som "converted.pdf".

**Konvertera HTML-fil till PDF:**
```bash
curl -X POST 
     -H "Content-Type: multipart/form-data" 
     -F "htmlFile=@path/to/your/file.html" 
     https://ApiUrl/api/pdf/convertHtmlFile
```

### Felhantering
API:t returnerar passande HTTP-statuskoder beroende p� om beg�ran lyckades eller misslyckades. 
Vid misslyckade beg�randen skickas �ven ett felmeddelande f�r att ge mer information om orsaken till felet.

- 400 Bad Request: Detta indikerar oftast att n�got i beg�randet inte var r�tt, t.ex. en ogiltig URL eller en icke-HTML-fil som skickades.
- 500 Internal Server Error: Detta antyder att n�got gick fel p� servern. Ytterligare detaljer kan finnas i svaret.
F�r b�sta praxis, se till att hantera dessa fel p� klientens sida f�r att ge anv�ndarna feedback och vidta l�mpliga �tg�rder.