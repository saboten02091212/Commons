// ******************************************************************
// ProcessMessage.cs ： Windowsメッセ―ジの一覧を保持するクラス。
// 作成日　：2013/05/30
// 更新履歴：2013/05/30 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuochi.Commons.Codes.Windows
{
    /// <summary>
    /// Windowsメッセ―ジの一覧を保持するクラスです。
    /// </summary>
    public static class ProcessMessage
    {
        #region 通常のウィンドウメッセージ
        /// <summary>
        /// ウィンドウのアクティブ化・非アクティブ化
        /// </summary>
        public const int WM_ACTIVATE = 0x0006;
        /// <summary>
        /// キーボードからの文字の入力
        /// </summary>
        public const int WM_CHAR = 0x0102;
        /// <summary>
        /// エディットコントロールのテキストの削除
        /// </summary>
        public const int WM_CLEAR = 0x0303;
        /// <summary>
        /// メニューアイテムの選択・コントロールからの通知
        /// </summary>
        public const int WM_COMMAND = 0x0111;
        /// <summary>
        /// コンテキストメニューを表示するために受け取る通知
        /// </summary>
        public const int WM_CONTEXTMENU = 0x007B;
        /// <summary>
        /// エディットコントロールのテキストのコピー
        /// </summary>
        public const int WM_COPY = 0x0301;
        /// <summary>
        /// エディットコントロールのテキストの切り取り
        /// </summary>
        public const int WM_CUT = 0x0300;
        /// <summary>
        /// ファイルがドロップされた
        /// </summary>
        public const int WM_DROPFILES = 0x0233;
        /// <summary>
        /// コントロールのフォントを取得
        /// </summary>
        public const int WM_GETFONT = 0x0031;
        /// <summary>
        /// ウィンドウのアイコンを取得
        /// </summary>
        public const int WM_GETICON = 0x007F;
        /// <summary>
        /// ウィンドウタイトルやコントロールのテキストを取得
        /// </summary>
        public const int WM_GETTEXT = 0x000D;
        /// <summary>
        /// ウィンドウタイトルやコントロールのテキストのサイズを取得
        /// </summary>
        public const int WM_GETTEXTLENGTH = 0x000E;
        /// <summary>
        /// 非システムキーが押された
        /// </summary>
        public const int WM_KEYDOWN = 0x0100;
        /// <summary>
        /// 押されていた非システムキーが離された
        /// </summary>
        public const int WM_KEYUP = 0x0101;
        /// <summary>
        /// マウス左ボタンをダブルクリック
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x0203;
        /// <summary>
        /// マウス左ボタンを押し下げ
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x0201;
        /// <summary>
        /// マウス左ボタンを離した
        /// </summary>
        public const int WM_LBUTTONUP = 0x0202;
        /// <summary>
        /// マウス中央ボタンをダブルクリック
        /// </summary>
        public const int WM_MBUTTONDBLCLK = 0x0209;
        /// <summary>
        /// マウス中央ボタンを押し下げ
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x0207;
        /// <summary>
        /// マウス中央ボタンを離した
        /// </summary>
        public const int WM_MBUTTONUP = 0x0208;
        /// <summary>
        /// メニューアイテムが選択された
        /// </summary>
        public const int WM_MENUSELECT = 0x011F;
        /// <summary>
        /// ウィンドウの移動
        /// </summary>
        public const int WM_MOVE = 0x0003;
        /// <summary>
        /// コモンコントロールからの通知
        /// </summary>
        public const int WM_NOTIFY = 0x004E;
        /// <summary>
        /// 効果をもたないメッセージ
        /// </summary>
        public const int WM_NULL = 0x0000;
        /// <summary>
        /// エディットコントロールのテキストの貼り付け
        /// </summary>
        public const int WM_PASTE = 0x0302;
        /// <summary>
        /// マウス右ボタンをダブルクリック
        /// </summary>
        public const int WM_RBUTTONDBLCLK = 0x0206;
        /// <summary>
        /// マウス右ボタンを押し下げ
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x0204;
        /// <summary>
        /// マウス右ボタンを離した
        /// </summary>
        public const int WM_RBUTTONUP = 0x0205;
        /// <summary>
        /// コントロールのフォントを設定
        /// </summary>
        public const int WM_SETFONT = 0x0030;
        /// <summary>
        /// ウィンドウタイトルやコントロールのテキストを設定
        /// </summary>
        public const int WM_SETTEXT = 0x000C;
        /// <summary>
        /// ウィンドウサイズ変更
        /// </summary>
        public const int WM_SIZE = 0x0005;
        /// <summary>
        /// システムメニューアイテム選択
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;
        /// <summary>
        /// エディットコントロールの直前の操作を元に戻す
        /// </summary>
        public const int WM_UNDO = 0x0304;
        /// <summary>
        /// アプリケーション定義メッセージの先頭
        /// </summary>
        public const int WM_USER = 0x0400;
        #endregion

        #region ステータスバーに送るメッセージ
        /// <summary>
        /// パーツの数と座標を取得
        /// </summary>
        public const int SB_GETPARTS = 0x0406;
        /// <summary>
        /// 表示されるテキストを取得
        /// </summary>
        public const int SB_GETTEXT = 0x0402;
        /// <summary>
        /// パーツの数と座標を設定
        /// </summary>
        public const int SB_SETPARTS = 0x0404;
        /// <summary>
        /// 表示されるテキストを設定
        /// </summary>
        public const int SB_SETTEXT = 0x0401;
        /// <summary>
        /// ステータスバーの表示モードを設定
        /// </summary>
        public const int SB_SIMPLE = 0x0409;
        #endregion

        #region リストビューに送るメッセージ
        /// <summary>
        /// すべてのアイテムを削除
        /// </summary>
        public const int LVM_DELETEALLITEMS = 0x1009;
        /// <summary>
        /// カラムを削除
        /// </summary>
        public const int LVM_DELETECOLUMN = 0x101C;
        /// <summary>
        /// アイテムを削除
        /// </summary>
        public const int LVM_DELETEITEM = 0x1008;
        /// <summary>
        /// アイテムを検索
        /// </summary>
        public const int LVM_FINDITEM = 0x100D;
        /// <summary>
        /// 背景色を取得
        /// </summary>
        public const int LVM_GETBKCOLOR = 0x1000;
        /// <summary>
        /// 拡張スタイルを取得
        /// </summary>
        public const int LVM_GETEXTENDEDLISTVIEWSTYLE = 0x1037;
        /// <summary>
        /// ヘッダコントロールを取得
        /// </summary>
        public const int LVM_GETHEADER = 0x101F;
        /// <summary>
        /// アイテムの属性を取得
        /// </summary>
        public const int LVM_GETITEM = 0x1005;
        /// <summary>
        /// アイテムの数を取得
        /// </summary>
        public const int LVM_GETITEMCOUNT = 0x1004;
        /// <summary>
        /// 指定した属性を持つアイテムを取得
        /// </summary>
        public const int LVM_GETNEXTITEM = 0x100C;
        /// <summary>
        /// テキストの文字色を取得
        /// </summary>
        public const int LVM_GETTEXTCOLOR = 0x1023;
        /// <summary>
        /// テキストの背景色を取得
        /// </summary>
        public const int LVM_GETTEXTBKCOLOR = 0x1025;
        /// <summary>
        /// 新しいカラム（列）を挿入
        /// </summary>
        public const int LVM_INSERTCOLUMN = 0x101B;
        /// <summary>
        /// 新しいアイテムを挿入
        /// </summary>
        public const int LVM_INSERTITEM = 0x1007;
        /// <summary>
        /// 背景色の設定
        /// </summary>
        public const int LVM_SETBKCOLOR = 0x1001;
        /// <summary>
        /// 拡張スタイルの設定
        /// </summary>
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = 0x1036;
        /// <summary>
        /// イメージリストの割り当て
        /// </summary>
        public const int LVM_SETIMAGELIST = 0x1003;
        /// <summary>
        /// アイテム・サブアイテムの属性を設定・変更
        /// </summary>
        public const int LVM_SETITEM = 0x1006;
        /// <summary>
        /// テキストの背景色を設定
        /// </summary>
        public const int LVM_SETTEXTBKCOLOR = 0x1026;
        /// <summary>
        /// テキストの文字色を設定
        /// </summary>
        public const int LVM_SETTEXTCOLOR = 0x1024;
        #endregion

        #region ツリービューに送るメッセージ
        /// <summary>
        /// アイテムを削除
        /// </summary>
        public const int TVM_DELETEITEM = 0x1101;
        /// <summary>
        /// アイテムを開く・閉じる
        /// </summary>
        public const int TVM_EXPAND = 0x1102;
        /// <summary>
        /// 背景色を取得
        /// </summary>
        public const int TVM_GETBKCOLOR = 0x111F;
        /// <summary>
        /// アイテム数の取得
        /// </summary>
        public const int TVM_GETCOUNT = 0x1105;
        /// <summary>
        /// アイテムの属性を取得
        /// </summary>
        public const int TVM_GETITEM = 0x110C;
        /// <summary>
        /// 指定されたアイテムを取得
        /// </summary>
        public const int TVM_GETNEXTITEM = 0x110A;
        /// <summary>
        /// 文字色を取得
        /// </summary>
        public const int TVM_GETTEXTCOLOR = 0x1120;
        /// <summary>
        /// 表示可能なアイテム数の取得
        /// </summary>
        public const int TVM_GETVISIBLECOUNT = 0x1110;
        /// <summary>
        /// 新しいアイテムを追加
        /// </summary>
        public const int TVM_INSERTITEM = 0x1100;
        /// <summary>
        /// イメージリストを設定
        /// </summary>
        public const int TVM_SETIMAGELIST = 0x1109;
        /// <summary>
        /// アイテムの属性を設定
        /// </summary>
        public const int TVM_SETITEM = 0x110D;
        /// <summary>
        /// 子アイテムのソート
        /// </summary>
        public const int TVM_SORTCHILDREN = 0x1113;
        #endregion

        #region タブコントロールに送るメッセージ
        /// <summary>
        /// ウィンドウ領域と表示領域を相互に変換
        /// </summary>
        public const int TCM_ADJUSTRECT = 0x1328;
        /// <summary>
        /// すべてのタブを削除
        /// </summary>
        public const int TCM_DELETEALLITEMS = 0x1309;
        /// <summary>
        /// タブを削除
        /// </summary>
        public const int TCM_DELETEITEM = 0x1308;
        /// <summary>
        /// 選択されているタブインデックスを取得
        /// </summary>
        public const int TCM_GETCURSEL = 0x130B;
        /// <summary>
        /// タブの情報を取得
        /// </summary>
        public const int TCM_GETITEM = 0x1305;
        /// <summary>
        /// タブの数を取得
        /// </summary>
        public const int TCM_GETITEMCOUNT = 0x1304;
        /// <summary>
        /// 新しいタブを挿入
        /// </summary>
        public const int TCM_INSERTITEM = 0x1307;
        /// <summary>
        /// タブを選択
        /// </summary>
        public const int TCM_SETCURSEL = 0x130C;
        /// <summary>
        /// イメージリストを設定
        /// </summary>
        public const int TCM_SETIMAGELIST = 0x1303;
        /// <summary>
        /// タブの属性を設定
        /// </summary>
        public const int TCM_SETITEM = 0x1306;
        #endregion

        #region アップダウンコントロールに送るメッセージ
        /// <summary>
        /// 現在のポジションを取得
        /// </summary>
        public const int UDM_GETPOS = 0x0468;
        /// <summary>
        /// 現在のポジション（32ビット値）を取得
        /// </summary>
        public const int UDM_GETPOS32 = 0x0472;
        /// <summary>
        /// バディウィンドウを設定
        /// </summary>
        public const int UDM_SETBUDDY = 0x0469;
        /// <summary>
        /// 現在のポジション（32ビット値）を設定
        /// </summary>
        public const int UDM_SETPOS = 0x0467;
        /// <summary>
        /// 現在のポジションを設定
        /// </summary>
        public const int UDM_SETPOS32 = 0x0471;
        /// <summary>
        /// ポジションの範囲を設定
        /// </summary>
        public const int UDM_SETRANGE = 0x0465;
        /// <summary>
        /// ポジションの範囲（32ビット値）を設定
        /// </summary>
        public const int UDM_SETRANGE32 = 0x046F;
        #endregion

        #region ツールバーに送るメッセージ
        /// <summary>
        /// ボタンイメージのリストにビットマップイメージを追加
        /// </summary>
        public const int TB_ADDBITMAP = 0x0413;
        /// <summary>
        /// 新しいボタンを追加
        /// </summary>
        public const int TB_ADDBUTTONS = 0x0414;
        /// <summary>
        /// 文字列のリストに新しい文字列を追加
        /// </summary>
        public const int TB_ADDSTRING = 0x041C;
        /// <summary>
        /// ツールバーサイズを調整
        /// </summary>
        public const int TB_AUTOSIZE = 0x0421;
        /// <summary>
        /// TBBUTTON構造体のサイズを設定
        /// </summary>
        public const int TB_BUTTONSTRUCTSIZE = 0x041E;
        /// <summary>
        /// ボタンを押された状態または押されていない状態に設定
        /// </summary>
        public const int TB_CHECKBUTTON = 0x0402;
        /// <summary>
        /// コマンド ID からボタンインデックスを取得
        /// </summary>
        public const int TB_COMMANDTOINDEX = 0x0419;
        /// <summary>
        /// ボタンを削除
        /// </summary>
        public const int TB_DELETEBUTTON = 0x0416;
        /// <summary>
        /// ボタンの選択可・不可を設定
        /// </summary>
        public const int TB_ENABLEBUTTON = 0x0401;
        /// <summary>
        /// ボタンの状態を取得
        /// </summary>
        public const int TB_GETSTATE = 0x0412;
        /// <summary>
        /// ボタンの表示・非表示を設定
        /// </summary>
        public const int TB_HIDEBUTTON = 0x0404;
        /// <summary>
        /// 新しいボタンを挿入
        /// </summary>
        public const int TB_INSERTBUTTON = 0x0415;
        /// <summary>
        /// システム定義のボタンイメージをロード
        /// </summary>
        public const int TB_LOADIMAGES = 0x0432;
        /// <summary>
        /// ビットマップのサイズを設定
        /// </summary>
        public const int TB_SETBITMAPSIZE = 0x0420;
        /// <summary>
        /// 無効状態にあるボタンのイメージリストを設定
        /// </summary>
        public const int TB_SETDISABLEDIMAGELIST = 0x0436;
        /// <summary>
        /// ホット状態にあるボタンのイメージリストを設定
        /// </summary>
        public const int TB_SETHOTIMAGELIST = 0x0434;
        /// <summary>
        /// デフォルト状態にあるボタンのイメージリストを設定
        /// </summary>
        public const int TB_SETIMAGELIST = 0x0430;
        /// <summary>
        /// インデントを設定
        /// </summary>
        public const int TB_SETINDENT = 0x042F;
        /// <summary>
        /// ボタンの状態を設定
        /// </summary>
        public const int TB_SETSTATE = 0x0411;
        #endregion

        #region リッチエディットコントロールに送るメッセージ
        /// <summary>
        /// 指定されたクリップボード形式のデータを貼り付けることができるかどうかを取得
        /// </summary>
        public const int EM_CANPASTE = 0x0432;
        /// <summary>
        /// 元に戻すことができるかどうかを取得
        /// </summary>
        public const int EM_CANUNDO = 0x00C6;
        /// <summary>
        /// 選択されている範囲を取得
        /// </summary>
        public const int EM_EXGETSEL = 0x0434;
        /// <summary>
        /// テキストサイズの上限を設定
        /// </summary>
        public const int EM_EXLIMITTEXT = 0x0435;
        /// <summary>
        /// 選択状態にする範囲を設定
        /// </summary>
        public const int EM_EXSETSEL = 0x0437;
        /// <summary>
        /// 文字列を検索
        /// </summary>
        public const int EM_FINDTEXT = 0x0438;
        /// <summary>
        /// 文字書式を取得
        /// </summary>
        public const int EM_GETCHARFORMAT = 0x043A;
        /// <summary>
        /// イベントマスクを取得
        /// </summary>
        public const int EM_GETEVENTMASK = 0x043B;
        /// <summary>
        /// 変更フラグ取得
        /// </summary>
        public const int EM_GETMODIFY = 0x00B8;
        /// <summary>
        /// オプションを取得
        /// </summary>
        public const int EM_GETOPTIONS = 0x044E;
        /// <summary>
        /// 段落書式を取得
        /// </summary>
        public const int EM_GETPARAFORMAT = 0x043D;
        /// <summary>
        /// 選択されている範囲を取得
        /// </summary>
        public const int EM_GETSEL = 0x00B0;
        /// <summary>
        /// 選択されているテキストを取得
        /// </summary>
        public const int EM_GETSELTEXT = 0x043E;
        /// <summary>
        /// 背景色を設定
        /// </summary>
        public const int EM_SETBKGNDCOLOR = 0x0443;
        /// <summary>
        /// 文字書式を設定
        /// </summary>
        public const int EM_SETCHARFORMAT = 0x0444;
        /// <summary>
        /// イベントマスクを設定
        /// </summary>
        public const int EM_SETEVENTMASK = 0x0445;
        /// <summary>
        /// 変更フラグを設定
        /// </summary>
        public const int EM_SETMODIFY = 0x00B9;
        /// <summary>
        /// オプションを設定
        /// </summary>
        public const int EM_SETOPTIONS = 0x044D;
        /// <summary>
        /// 段落書式を設定
        /// </summary>
        public const int EM_SETPARAFORMAT = 0x0447;
        /// <summary>
        /// 内容をストリームに置き換える
        /// </summary>
        public const int EM_STREAMIN = 0x0449;
        /// <summary>
        /// 内容をストリームに書き出す
        /// </summary>
        public const int EM_STREAMOUT = 0x044A;
        /// <summary>
        /// 直前の操作を元に戻す
        /// </summary>
        public const int EM_UNDO = 0x00C7;
        #endregion
    }
}
