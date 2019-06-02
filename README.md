# MyKeyChanMyKeyChangerForAppleWirelessger
キーマッピングの変更ツール


## 概要
Apple Wireless Keyboardのキーマッピング変更ツール。Planckキーボードを使用していたのですが、打鍵感はApple Wireless Keyboardの方が好きだったので、Planckキーボードで使用していたキーマッピングに近いものを実現することを目的としています。

いくつかのキーはレジストリを変更してキーマッピングを変更しています。変更内容は doc/registry を参照してもらえればと思います。
* FNKeyChange.reg → FnキーなしでF1～F12を使用するためのレジストリ変更
* FNKeyDefault → FNKeyChange.regの設定をリセットするためのレジストリ変更(レジストリキーの削除でも良さそうです)
* KeyMappingI v10 (Apple WiressKeyboard).reg → キーマッピングの変更
* KeyMappingI v10 (Apple WiressKeyboard).xlsx → KeyMappingI v10 (Apple WiressKeyboard).regの元ネタ

## やっていること
入力されたキーをフックして別のキーを代わりに送信


## キーマッピング
![00_base](https://user-images.githubusercontent.com/31182578/58758730-89fb2800-855a-11e9-8674-c57ec695581d.jpg)
![01_User1](https://user-images.githubusercontent.com/31182578/58758734-a26b4280-855a-11e9-88d5-55af395a8887.jpg)
![02_User2](https://user-images.githubusercontent.com/31182578/58758737-ab5c1400-855a-11e9-88c1-d0b005b2a04c.jpg)
![03_User3](https://user-images.githubusercontent.com/31182578/58758738-b2832200-855a-11e9-8897-74a98778b2fe.jpg)


## 制限
* キー入力を監視しているのでキー入力の反応が遅れる可能性があります。 
* 十分なデバッグは行っていないので予期せぬ動作を起こす可能性があります。
* Altとの組み合わせ(例えばAlt + F4)が動作しないことを確認しています(ファンクションキーにマッピングしたキーを使用した場合の話です)。

