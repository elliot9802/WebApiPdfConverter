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
API:n erbjuder en enkel slutpunkt f�r att skicka en webbadress (URL) och f� tillbaka en PDF-representation av webbsidan.

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
Om det �r framg�ngsrikt f�r du tillbaka en PDF-representation av webbsidan som en byte-stream.

### Exempel
Anv�nd f�ljande curl-kommando f�r att testa API:t:
```bash
curl -X POST "https://localhost:7099/api/pdf/convertUrl" \
     -H "Content-Type: application/json" \
     -d '{"Url": "https://www.example.com"}' \
     --output converted.pdf
```
Detta kommando kommer att skicka en f�rfr�gan till API:t f�r att konvertera "https://www.example.com" till PDF och spara den som "converted.pdf".

### Felhantering
Om n�got g�r fel n�r du f�rs�ker konvertera en URL till PDF, kommer API:t att svara med ett relevant felmeddelande som hj�lper dig att f�rst� problemet.

