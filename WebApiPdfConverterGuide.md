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
API:n erbjuder en enkel slutpunkt för att skicka en webbadress (URL) och få tillbaka en PDF-representation av webbsidan.

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
Om det är framgångsrikt får du tillbaka en PDF-representation av webbsidan som en byte-stream.

### Exempel
Använd följande curl-kommando för att testa API:t:
```bash
curl -X POST "https://localhost:7099/api/pdf/convertUrl" \
     -H "Content-Type: application/json" \
     -d '{"Url": "https://www.example.com"}' \
     --output converted.pdf
```
Detta kommando kommer att skicka en förfrågan till API:t för att konvertera "https://www.example.com" till PDF och spara den som "converted.pdf".

### Felhantering
Om något går fel när du försöker konvertera en URL till PDF, kommer API:t att svara med ett relevant felmeddelande som hjälper dig att förstå problemet.

