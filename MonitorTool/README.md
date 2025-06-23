# 🖥️ Monitor Presets Switcher

## Türkçe Açıklama

### Proje Amacı
Windows işletim sisteminde birden fazla monitörün ekran ayarlarını (çözünürlük, yenileme hızı, konum, renk derinliği ve yön) birer "taslak (preset)" olarak kaydedip, bu taslaklar arasında kolayca geçiş yapmanızı sağlar.

### Özellikler
- Çoklu monitör desteği
- Sınırsız preset kaydı
- Her preset için masaüstüne otomatik kısayol oluşturma
- Çözünürlük, yenileme hızı, pozisyon, renk derinliği (bitsPerPel) ve yön (dikey/yatay/orientation) desteği
- Komut satırı ile kolay kullanım

### Kullanım

#### 1. Mevcut ayarları kaydetmek:
```sh
MonitorTool.exe
```
Programı çalıştırınca sizden bir preset adı ister ve mevcut monitör ayarlarını kaydeder, masaüstüne kısayol oluşturur.

#### 2. Kayıtlı bir ayarı uygulamak:
```sh
MonitorTool.exe use presetAdi
```
veya masaüstündeki kısayola çift tıklayın.

#### 3. JSON formatı örneği
```json
{
  "monitors": [
    {
      "deviceName": "\\.\\DISPLAY1",
      "width": 2560,
      "height": 1440,
      "hz": 59,
      "posX": -2560,
      "posY": -568,
      "bitsPerPel": 32,
      "orientation": 0
    }
  ]
}
```
- orientation: 0=Landscape, 1=Portrait, 2=Landscape (flipped), 3=Portrait (flipped)

### Gereksinimler
- Windows 10/11
- .NET 8.0 Runtime

---

## English Description

### Project Purpose
This tool allows you to save the display settings (resolution, refresh rate, position, color depth and orientation) of multiple monitors on Windows as named "presets" and switch between them easily.

### Features
- Multi-monitor support
- Unlimited preset saving
- Automatic desktop shortcut creation for each preset
- Supports resolution, refresh rate, position, color depth (bitsPerPel) and orientation (landscape/portrait)
- Simple command-line usage

### Usage

#### 1. Save current settings as a preset:
```sh
MonitorTool.exe
```
When you run the program, it will ask for a preset name and save the current monitor settings, creating a shortcut on your desktop.

#### 2. Apply a saved preset:
```sh
MonitorTool.exe use presetName
```
Or simply double-click the shortcut on your desktop.

#### 3. Example JSON format
```json
{
  "monitors": [
    {
      "deviceName": "\\.\\DISPLAY1",
      "width": 2560,
      "height": 1440,
      "hz": 59,
      "posX": -2560,
      "posY": -568,
      "bitsPerPel": 32,
      "orientation": 0
    }
  ]
}
```
- orientation: 0=Landscape, 1=Portrait, 2=Landscape (flipped), 3=Portrait (flipped)

### Requirements
- Windows 10/11
- .NET 8.0 Runtime 