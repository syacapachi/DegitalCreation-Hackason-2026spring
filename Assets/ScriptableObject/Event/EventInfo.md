# ScriptableObject Event 一覧と役割

このドキュメントでは、`Assets/ScriptableObject/Event/` に実装・配置されているすべてのイベント（ScriptableObject）の名前、型、およびその役割についてまとめています。

## 📸 カメラ・写真関連 (CameraCapture / PhotoManager)

| アセット名 | クラス型 | 役割・用途 |
| :--- | :--- | :--- |
| **ShutterEvent** | `VoidEventSO` | カメラのシャッターが切られた瞬間に発火します。主にシャッター音の再生や画面のフラッシュ（暗転）エフェクト（`UIPresenter`等）に使用されます。 |
| **PhotoDataEvent** | `PhotoDataEvent` | 写真の撮影および処理が完了した際に発火し、画像や被写体情報を含む `PhotoData` を通知します。UIへの反映や写真リストへの追加に使用されます。 |
| **CaptureFailedEvent** | `VoidEventSO` | 何らかの理由（テクスチャの読み取りエラー等）で写真の撮影に失敗した際に発火します。 |
| **BurstProgressEvent** | `BurstProgressEvent` | 連写（バースト）撮影中の進捗（現在何枚目か / 全体で何枚か）を通知します。UIの連写進捗表示に使用されます。 |
| **BurstFinished** | `VoidEventSO` | 連写（バースト）撮影がすべて完了したタイミングで発火します。 |
| **OnPhotoReachMax** | `VoidEventSO` | 写真の保存上限（MaxPhotos）に達している状態でさらに撮影を行おうとした際に、`PhotoManager` から発火します。 |

## ⏱️ ゲーム進行・タイマー関連 (GameManager)

| アセット名 | クラス型 | 役割・用途 |
| :--- | :--- | :--- |
| **OnCountDownStart** | `VoidEventSO` | ゲームのカウントダウンタイマーが開始されたタイミングで発火します。 |
| **CountDownProgress** | `BurstProgressEvent` | カウントダウンの残り時間（現在の秒数 / 全体の秒数）を1秒ごとに通知します。バッテリーゲージやタイマーのUI（`UIPresenter`）を更新するために使用されます。 |
| **OnCountDownEnd** | `VoidEventSO` | カウントダウンタイマーが0になった（ゲーム終了）タイミングで発火します。主にリザルト画面の表示（`UIPresenter`）などに使用されます。 |

## 🔥 インゲームイベント関連

| アセット名 | クラス型 | 役割・用途 |
| :--- | :--- | :--- |
| **OnFireEventStarted** | `VoidEventSO` | ゲーム内で「火災イベント」が開始されたタイミング（残り60秒時など）で発火します。`FireSpawner` 等がこれを受け取り、火災オブジェクトを生成・有効化します。 |
| **OnFireEventCaptured** | `VoidEventSO` | 火災イベントのターゲット（`PhotoTargetType.FireEvent`）の撮影に成功した際に発火します。ボーナススコアのUI表示などに使用されます。 |

## ⚙️ UI・設定関連

| アセット名 | クラス型 | 役割・用途 |
| :--- | :--- | :--- |
| **OnSeisitivityValueChanged** | `FloatEventSO` | 設定画面でマウス感度などのスライダー値が変更された際に発火し、変更後の数値を通知します。（設定のデータバインディング用） |
