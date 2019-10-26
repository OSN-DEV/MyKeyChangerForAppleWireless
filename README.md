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
![00_base](https://github.com/OSN-DEV/MyKeyChangerForAppleWireless/blob/master/doc/keymaping/00_base.jpg?raw=true)
![01_User1](https://github.com/OSN-DEV/MyKeyChangerForAppleWireless/blob/master/doc/keymaping/01_User1.jpg?raw=true)
![02_User2](https://github.com/OSN-DEV/MyKeyChangerForAppleWireless/blob/master/doc/keymaping/02_User2.jpg?raw=true)
![03_User3](https://github.com/OSN-DEV/MyKeyChangerForAppleWireless/blob/master/doc/keymaping/03_User3.jpg?raw=true)
![04_User4](https://github.com/OSN-DEV/MyKeyChangerForAppleWireless/blob/master/doc/keymaping/04_User4.jpg?raw=true)
![05_User5](https://github.com/OSN-DEV/MyKeyChangerForAppleWireless/blob/master/doc/keymaping/05_User5.jpg?raw=true)


## 制限
* キー入力を監視しているのでキー入力の反応が遅れる可能性があります。 
* 十分なデバッグは行っていないので予期せぬ動作を起こす可能性があります。
* Altとの組み合わせ(例えばAlt + F4)が動作しないことを確認しています(ファンクションキーにマッピングしたキーを使用した場合の話です)。
* Alt以外でもキーの組み合わせによって動作しないことを確認しています。とはいえ、そもそもイベントをフックできないので対処方法が見いだせていません。。

## 更新履歴
### 2019.10.26
かな入力の濁音に対応。

### 2019.07.27
User2のキー配置を一部変更。

### 2019.06.02
英数 + Shift + E が認識しないので(理由は不明)、英数 + Z のアサインを変更。


