# WebApiPdfConverter API-guide

## Innehållsförteckning
1. [Översikt](#översikt)
2. [Installation](#installation)
   - [Förutsättningar](#förutsättningar)
   - [Steg-för-steg installation](#steg-för-steg-installation)
3. [Användning](#användning)
   - [Konvertera URL till PDF](#konvertera-url-till-pdf)
   - [Konvertera HTML till PDF](#konvertera-html-fil-till-pdf)
   - [Exempel](#exempel)
<!--
4. [Felhantering](#felhantering)
5. [Caching](#caching)
-->
## Översikt
WebApiPdfConverter är ett webb-API som tillhandahåller tjänster för att konvertera webbplatser och HTML-innehåll till PDF-filer. 
API:t erbjuder enkla slutpunkter för att skicka en webbadress (URL) eller en HTML-fil och få tillbaka en PDF-representation.
API:t är byggt för att generera biljetter för olika evenemang och anläggningar, som t.ex. museer och simhallar, inom ett svenskt boknings-, bidrags- och besökssystemet.

## Installation
För en framgångsrik installation av WebApiPdfConverter, 
se till att uppfylla de angivna förutsättningarna och följ sedan installationsstegen noggrant.

### Förutsättningar
- .NET Core installerat på din dator.
- En IDE som stöder C# och .NET Core, t.ex. Visual Studio.

### Steg-för-steg installation
1. Klona eller ladda ner koden från git-repot.
2. Öppna lösningen i din IDE.
3. Återställ alla NuGet-paket om det behövs. (Syncfusion.HtmlToPdfConverter.Net.Windows, Syncfusion.Pdf.Net.Core och alla saknade System/Microsoft extensions som kommer säga ifrån)
4. Bygg projektet för att säkerställa att det inte finns några kompileringsfel.
5. Kör API:t antingen genom att trycka på "Start" i din IDE eller genom att använda terminalen och skriva `dotnet run`.

## Användning

### Konvertera URL till PDF
Använd POST-metoden med slutpunkten /api/pdf/convertUrl och skicka URL:en som ska konverteras i JSON-format.

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
Använd POST-metoden med slutpunkten /api/pdf/convertHtmlFile och skicka HTML-filen som multipart/form-data.

- **Metod:** POST
- **Slutpunkt:** `/api/pdf/convertHtmlFile`

**Payload:** 
En `multipart/form-data`-begäran med en filparameter `htmlFile` som innehåller HTML-filen som ska konverteras. <br/>
**Svar:** En PDF-fil som representerar innehållet i den uppladdade HTML-filen.

### Exempel

**Konvertera URL till PDF:**
För att konvertera en URL till PDF, använd följande curl-kommando:
```bash
curl -X POST "https://ApiUrl/api/pdf/convertUrl" \
     -H "Content-Type: application/json" \
     -d '{"Url": "https://www.example.com"}' \
     --output converted.pdf
```
Detta kommando kommer att skicka en förfrågan till API:t för att konvertera "https://www.example.com" till PDF och spara den som "converted.pdf".

**Konvertera HTML-fil till PDF:**
För att konvertera en HTML till PDF, använd följande curl-kommando:
```bash
curl -X POST "https://ApiUrl/api/pdf/convertHtmlFile" \
     -H "Content-Type: multipart/form-data" \
     --output converted.pdf
```
Detta kommando kommer att skicka en förfrågan till API:t för att konvertera inmatad fil till PDF och spara den som "converted.pdf".
<!-- 
### Felhantering
WebApiPdfConverter API:et använder ett robust felhanteringssystem för att säkerställa att klienter får tydlig och användbar feedback när något går fel under konverteringsprocessen. 
Felhantering i API:et innefattar flera nivåer av kontroller och svar som hjälper till att diagnostisera och lösa problem effektivt.

När ett fel uppstår returnerar API:et en HTTP-statuskod tillsammans med ett felmeddelande i JSON-format som beskriver problemets natur. 
Nedan följer en detaljerad beskrivning av hur API:et hanterar olika typer av fel:

- **400 Bad Request:** Detta fel indikerar att klientens förfrågan inte kunde bearbetas på grund av ogiltig syntax. 
Det kan bero på felaktig datastruktur, ogiltiga URL:er, eller felaktigt formaterad JSON. 
API:et ger ett detaljerat felmeddelande som specificerar den exakta orsaken till problemet, vilket gör det möjligt för klienten att korrigera och återsända begäran.

- **401 Unauthorized:** Om en begäran kräver autentisering och den inte tillhandahålls eller är ogiltig, returneras detta fel. 
Detta säkerställer att endast auktoriserade användare kan utföra konverteringar.

-  **403 Forbidden:** Detta fel returneras när servern förstår begäran men vägrar att auktorisera den. 
Detta kan inträffa om användaren inte har rättigheter för den begärda operationen.

- **404 Not Found:** Om den begärda resursen inte kan hittas, till exempel en URL som inte existerar, returneras detta fel. 
Felmeddelandet kommer att uppmana klienten att kontrollera URL:en och försöka igen.

- **500 Internal Server Error:** Detta är ett generellt fel som indikerar att något har gått fel på servern som inte kan specificeras närmare. 
Det kan röra sig om allt från databasfel till oväntade undantag i koden. API:et loggar dessa fel internt med fullständig stackspårning för att underlätta felsökning för utvecklare.

- **503 Service Unavailable:** Detta fel indikerar att servern för tillfället är oförmögen att hantera begäran på grund av tillfällig överbelastning eller underhåll. Klienter bör försöka igen senare.

För varje feltyp loggar API:et detaljerad information, inklusive tidpunkt, begäran som orsakade felet, och en stackspårning där det är tillämpligt. Detta underlättar snabb diagnos och åtgärd av problem. 
Utvecklare rekommenderas att granska loggarna regelbundet för att identifiera och åtgärda återkommande problem.

Klienter bör implementera lämplig felhantering baserat på dessa svar för att informera användarna om problemet och vidta nödvändiga åtgärder. 
Det är också viktigt att klienter hanterar potentiella nätverksproblem eller timeout-fel som kan uppstå under kommunikationen med API:et.

Genom att följa dessa riktlinjer kan klienter skapa en mer robust och tillförlitlig integration med WebApiPdfConverter API:et, vilket säkerställer en smidig användarupplevelse även när fel uppstår.

### Caching
API:t implementerar caching för att effektivisera omvandlingen av återkommande data. 
Caching används för att minska belastningen och förbättra responsiviteten. Cachingstrategin innebär att data lagras i en timme för att snabbt kunna leverera PDF:er med data som ofta begärs.

För detaljerad information om API-anrop, felkoder och exempel, se de specifika avsnitten ovan. -->
