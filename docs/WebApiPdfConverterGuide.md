# WebApiPdfConverter API-guide

## Innehållsförteckning
1. [Översikt](#översikt)
2. [Installation](#installation)
   - [Förutsättningar](#förutsättningar)
   - [Steg-för-steg installation](#steg-för-steg-installation)
3. [Användning](#användning)
   - [Konvertera URL till PDF](#konvertera-url-till-pdf)
   - [Exempel](#exempel)
4. [Felhantering](#felhantering)

## Översikt
WebApiPdfConverter är en webb-API som låter användare konvertera webbplatser till PDF-filer. 
API:n erbjuder enkla slutpunkter för att skicka en webbadress (URL) eller en HTML-fil och få tillbaka en PDF-representation.

## Installation
För en framgångsrik installation av WebApiPdfConverter, 
se till att uppfylla de angivna förutsättningarna och följ sedan installationsstegen noggrant.

### Förutsättningar
- .NET Core installerat på din dator.
- En IDE som stöder C# och .NET Core, t.ex. Visual Studio.

### Steg-för-steg installation
1. Klona eller ladda ner koden från git-repot.
2. Öppna lösningen i din IDE.
3. Återställ alla NuGet-paket om det behövs.
4. Bygg projektet för att säkerställa att det inte finns några kompileringsfel.
5. Kör API:t antingen genom att trycka på "Start" i din IDE eller genom att använda terminalen och skriva `dotnet run`.

## Användning

### Konvertera URL till PDF
För att konvertera en webbadress (URL) till PDF, använd följande slutpunkt:

- **Metod:** POST
- **Slutpunkt:** `/api/pdf/convertUrl`

**Payload:**
```json
{
    "Url": "<din_webbplats_url>"
}
```
**Svar:**
En PDF-fil som representerar den angivna webbadressens innehåll.

### Konvertera HTML-fil till PDF
För att konvertera en uppladdad HTML-fil till PDF, använd följande slutpunkt:

- **Metod:** POST
- **Slutpunkt:** `/api/pdf/convertHtmlFile`

**Payload:** 
En `multipart/form-data`-begäran med en filparameter `htmlFile` som innehåller HTML-filen som ska konverteras.
**Svar:** En PDF-fil som representerar innehållet i den uppladdade HTML-filen.

### Exempel

**Konvertera URL till PDF:**
```bash
curl -X POST 
     -H "Content-Type: application/json" 
     -d '{"Url": "http://example.com"}' 
     https://ApiUrl/api/pdf/convertUrl
```

### Exempel
Använd följande curl-kommando för att testa API:t:
```bash
curl -X POST "https://ApiUrl/api/pdf/convertUrl" \
     -H "Content-Type: application/json" \
     -d '{"Url": "https://www.example.com"}' \
     --output converted.pdf
```
Detta kommando kommer att skicka en förfrågan till API:t för att konvertera "https://www.example.com" till PDF och spara den som "converted.pdf".

**Konvertera HTML-fil till PDF:**
```bash
curl -X POST 
     -H "Content-Type: multipart/form-data" 
     -F "htmlFile=@path/to/your/file.html" 
     https://ApiUrl/api/pdf/convertHtmlFile
```

### Felhantering
API:t returnerar passande HTTP-statuskoder beroende på om begäran lyckades eller misslyckades. 
Vid misslyckade begäranden skickas även ett felmeddelande för att ge mer information om orsaken till felet.

- 400 Bad Request: Detta indikerar oftast att något i begärandet inte var rätt, t.ex. en ogiltig URL eller en icke-HTML-fil som skickades.
- 500 Internal Server Error: Detta antyder att något gick fel på servern. Ytterligare detaljer kan finnas i svaret.
För bästa praxis, se till att hantera dessa fel på klientens sida för att ge användarna feedback och vidta lämpliga åtgärder.