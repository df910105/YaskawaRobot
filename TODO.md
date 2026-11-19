# TODO

最後更新:2026-11-19

## NuGet 發佈

套件名稱 `YaskawaRobot.YHSES`(assembly 仍為 `YRCC`),目前版本 `1.0.0-beta.1`。

- [x] 套件中繼資料、單一版本來源(`Version`)、XML 文件、snupkg、SourceLink(`chore/nuget-packaging`)
- [x] 本機 `dotnet pack -c Release` 驗證內容(net472 + netstandard2.0,含 CodePages 相依)
- [ ] 申請 nuget.org 帳號與 API key(不要放進 repo)
- [ ] 以 `1.0.0-beta.1` 發佈試水:`dotnet nuget push <nupkg> -k <key> -s https://api.nuget.org/v3/index.json`
- [ ] README 補上 NuGet 安裝說明,並修正範例(`rt`、`err` 未宣告,無法直接複製)
- [ ] 加 GitHub Actions:build + pack,tag 時自動發佈(只 pack `YRCC/YRCC.csproj`,不要 pack `Example`)
- [ ] 確認 hsinkoyu/fs100 的授權允許參考使用,並在套件說明中標明

## 發佈 1.0.0 之前的 API 問題

公開 API 一旦發佈就難以更動,建議在 1.0.0 前處理:

- [ ] `Posistion` / `BasePosistion` 拼字錯誤(`Position`),會是破壞性變更
- [ ] `ERROR_CONNECTION = 1` 與控制器狀態值 1 無法區分(傳輸錯誤與控制器 status 共用 `int` 回傳值)
- [ ] `req_id` 永遠為 0,回應沒有與請求配對(逾時後遲到的封包可能被下一個請求讀到)
- [ ] `DisplayMessage` 沒有長度上限檢查(需先確認協定的最大長度)
- [ ] 各 `Read*` 解碼沒有檢查 `data.Length`,短回應會丟 `ArgumentException`
- [ ] 沒有任何自動化測試;只在 YRC1000 測過,DX200 / FS100 未測

## 功能

- [ ] 尚未啟用的指令:`RCC07`(位置誤差)、`RCC08`(扭矩)、`RCC17`(基座位置)、`RCC18`(外部軸)、`FCC01`(檔案控制)。README 寫 43/47 完成,其餘為這些註解掉的項目
- [ ] `RCC17` 啟用前要處理 `BasePosistion.AxisData` 預設為 null 的問題
- [ ] `WriteStrData` 的 16 bytes 上限是依 Big5 長度;其他 `PendantEncoding` 的欄位長度需確認

## 文件

- [ ] `CLAUDE.md` 補上:`YHSES` 實作 `IDisposable`;套件發佈流程(完成後)
