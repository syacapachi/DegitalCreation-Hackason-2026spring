# DegitalCreation-Hackason-2026spring

デジタルクリエイション系ハッカソン向けの Unity プロジェクトです。街・自然などの3D環境を**移動・飛行**しながら**カメラで撮影**し、写った被写体に応じて**スコア**が加算されるゲーム向けの構成になっています。

## 動作環境

- **Unity**: `6000.3.6f1`（`ProjectSettings/ProjectVersion.txt` 準拠）
- **レンダーパイプライン**: Universal Render Pipeline（`com.unity.render-pipelines.universal`）
- **入力**: [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest)（`Move` / `Look` / `Accelarate` アクション）

## ビルドに含まれるシーン

`ProjectSettings/EditorBuildSettings.asset` より:

| 順序 | シーンパス | 備考 |
|------|------------|------|
| 0 | `Assets/Scenes/SampleScene.unity` | テンプレート由来のサンプル |
| 1 | `Assets/Scenes/GameScene.unity` | メインゲーム想定 |
| 2 | `Assets/Scenes/Home.unity` | タイトル／ホーム。開始で `GameScene` へ遷移 |

その他、`Assets/Scenes/systemLogicScene.unity`・`Assets/Scenes/koki/koki.unity` など開発用・検証用シーンが同梱されています（ビルド設定への登録は任意）。

## ゲームの流れ（自作スクリプトから読み取れる範囲）

1. **ホーム**（`HomePresenter`）で開始すると `GameScene` をロード。
2. **プレイ**中、`PlayerInputReciever` で移動・視点・ブースト入力を受け取り、`MoveController` が地上移動または**飛行物理**（揚力・空気抵抗・失速など）を担当。`MouseCameraController` がマウス視点を更新。
3. `CameraCapture` が **単発／連写（バースト）** でレンダーターゲットから画像を取得し、`PhotoAnalyzer` で画面内の `Renderer` を解析。`PhotoTargetController`＋`PhotoTargetDataSO` が付いたオブジェクトを被写体としてスコア要素に反映。
4. `PhotoManager` が枚数上限（既定 `maxPhotos`）まで写真データを保持。
5. `UIPresenter` が**制限時間カウントダウン**・残り枚数表示・シャッター演出（DOTween / UniTask）などを担当。終了でリザルト UI を表示。
6. `ResultPanelModel` が撮影完了ごとにスコアを累積し、`ResultPanelPresenter` が結果表示。リザルト時に JSON 形式でプレイ履歴（スコア・ニックネームなど）が保存され、ホームへ戻るで `Home` シーンへ。
7. **ランキング・設定**: `RankingManager` により過去の成績がランキング表示されるほか、設定画面を通じた動的なキーバインド変更（`PlayerPrefs` への永続化）も実装されています。

**オーディオ**: `ManagerLocator`（シーンを跨いで生存）が `AudioManager` を参照し、カウントダウン開始で BGM、シャッターで効果音を再生。

## `Assets` フォルダ構成（概要）

| フォルダ | 内容 |
|----------|------|
| `Script/` | 本プロジェクトの主なゲームロジック（UI・写真・キャラ・設定・マネージャなど） |
| `ScriptableObject/` | `PhotoTargetDataSO` などアセット定義の保存先 |
| `Scenes/` | `Home` / `GameScene` / `SampleScene` など |
| `Prefabs/`・`prefabs/` | UI、AudioManager、プレイヤールート、チュートリアル・撮影ターゲット例など |
| `Editor/` | Inspector 拡張（`EnableIf` 系、`OnInspectorButton`、`SerializeReference` 表示など） |
| `Plugins/` | DOTween などサードパーティプラグイン |
| `Settings/` | URP レンダラー等のプロジェクト設定アセット |
| `ZRNAssets/` | 都市モデル・Query-Chan・車両デモなど ZRN 系アセットと付属スクリプト |
| `OutPost/` | 街並み（例: NYC ブロック）と関連 Prefab／シーン |
| `Stylized Nature Environment/`・`Foliage Free/` | 自然系モデル・シーン例 |
| `Kevin Iglesias/` | Human Character Dummy（人型ダミー） |
| `TextMesh Pro/` | TMP 標準リソース |
| `AudioFile/`・`Images/`・`Sprite/`・`Texture/`・`Material/` など | メディア・マテリアル類 |
| `TutorialInfo/` | Unity テンプレート付属のチュートリアル用ウィンドウ |

## 主要スクリプト（`Assets/Script`）

| 領域 | 代表クラス | 役割 |
|------|------------|------|
| マネージャ | `Syacapachi.Manager.ManagerLocator` | `AudioManager` の参照と `DontDestroyOnLoad` |
| 入力・移動 | `PlayerInputReciever`, `MoveController`, `MouseCameraController` | Input System、飛行／歩行、カメラ回転 |
| 撮影 | `Syacapachi.Camera.CameraCapture`, `PhotoManager` | キャプチャ、バースト、写真リスト管理 |
| 解析・スコア | `PhotoAnalyzer`, `PhotoTargetController`, `ResultPanelModel` | 被写体検出とスコア加算 |
| UI | `UIPresenter`, `UIView`, `HomePresenter`, `ResultPanelPresenter`, チュートリアル | 画面遷移と表示制御（MVPパターンベース） |
| ランキング | `RankingManager`, `RankingPresenter`, `RankingView` | JSON形式でのスコア履歴の永続化と、ランキングUI表示 |
| オーディオ | `AudioManager` | BGM・シャッター音 |
| ユーティリティ | `StaticMeshColliderBaker` | SkinnedMesh の現在ポーズを MeshCollider に焼き付け |
| 設定（Koki） | `SettingManager`, `SettingView`, `SettingPresenter`, `SettingSO` | 設定画面（キーバインド等）と `PlayerPrefs` を使った永続化 |

カスタム属性（`Assets/Script/Attribute/`）と `Assets/Editor/` の Drawer により、Inspector 上で条件表示やボタン実行が可能です。

## パッケージ（抜粋）

`Packages/manifest.json` より、ゲーム実装で特に効いているもの:

- **UniTask**（`com.cysharp.unitask`）
- **Input System**
- **Universal RP**
- **ugui**（UI）

※ **DOTween** は `Assets/Plugins/Demigiant/DOTween` として同梱されています。

## ライセンス・クレジット

`ZRNAssets`、`OutPost`、Asset Store／フリーアセット（Stylized Nature、Foliage Free、Kevin Iglesias、TextMesh Pro、DOTween 等）には各パッジの利用条件があります。配布・公開前に各フォルダ内のライセンス表記を確認してください。

---

リポジトリ名のスペルは `Digital` ではなく `Degital` となっています（既存名に合わせています）。
