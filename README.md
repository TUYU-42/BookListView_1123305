
# 📚 圖書管理程式 BookListView

一款使用 C# Windows Forms 開發的圖書管理系統，透過 **ListView 清單檢視控制項** 展示書籍資訊，支援多種檢視模式切換及借書功能。

## 功能簡介

- **多種檢視模式**：大圖示、詳細資料、小圖示、清單、大圖示加詳細資料（Tile），透過下拉選單即時切換
- **書籍封面顯示**：每本書籍搭配彩色封面圖示，大圖示模式下以直書方式呈現書名
- **借書功能**：雙擊（TwoClick）書籍項目彈出確認對話框，確認後加入借書清單
- **重複借閱檢查**：已借閱書籍不會重複加入清單
- **詳細資料欄位**：包含書名、作者、類別三個欄位

## 書籍資料

| 書名       | 作者   | 類別     |
|----------|--------|--------|
| 三國演義   | 羅貫中   | 章回小說 |
| 西遊記     | 吳承恩   | 章回小說 |
| 唐詩三百首 | 孫洙     | 詩選     |
| 楚辭       | 劉向     | 詩歌     |
| 西廂記     | 王實甫   | 戲曲     |
| 水滸傳     | 施耐庵   | 章回小說 |
| 紅樓夢     | 曹雪芹   | 章回小說 |
| 牡丹亭     | 湯顯祖   | 戲曲     |

## 環境需求

- .NET 8.0 SDK（或以上）
- Windows 作業系統（Windows Forms）
- Visual Studio 2022（建議）

## 執行方式

```bash
# 複製專案後進入資料夾
cd BookListView

# 編譯並執行
dotnet run
```

或在 Visual Studio 中開啟 `BookListView.csproj`，按 **F5** 執行。

## 程式截圖

### 大圖示模式
<img width="808" height="560" alt="螢幕擷取畫面 2026-05-20 011748" src="https://github.com/user-attachments/assets/17887190-e9a5-4a52-8d0a-ccea6dc5d53b" />

### 詳細資料模式
<img width="824" height="382" alt="螢幕擷取畫面 2026-05-20 011808" src="https://github.com/user-attachments/assets/c6caa575-6ef5-4bb4-a8cd-761639448ffc" />


### 借書確認對話框
<img width="553" height="360" alt="螢幕擷取畫面 2026-05-20 011824" src="https://github.com/user-attachments/assets/fa40a500-9886-42df-adfb-fa5f3582b8d0" />

## 專案結構

```
BookListView/
├── BookListView.csproj    # 專案檔
├── Program.cs             # 程式進入點
├── frmBooks.cs            # 主視窗（ListView + 借書邏輯）
├── .gitignore             # Git 忽略規則
└── README.md              # 本文件
```

## 控制項配置

| 控制項      | 名稱        | Dock   | 說明                          |
|------------|------------|--------|-------------------------------|
| Form       | frmBooks   | —      | 主視窗                        |
| ListView   | lvwBooks   | Fill   | 書籍清單（LargeImageList: imgL, SmallImageList: imgS） |
| Panel      | pnlTools   | Right  | 右側工具面板                    |
| GroupBox   | grpView    | Top    | 檢視方式選擇區                  |
| ComboBox   | cmbView    | Fill   | 檢視模式下拉選單                |
| GroupBox   | grpBorrow  | Fill   | 借書清單區                      |
| ListBox    | lstBorrow  | Fill   | 已借書籍清單                    |
| ImageList  | imgL       | —      | 大圖示（90×120）               |
| ImageList  | imgS       | —      | 小圖示（15×20）                |

## 授權

本專案為課程作業用途。
