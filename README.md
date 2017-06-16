# Gainscha
佳博打印机ESC打印机SDK
使用方法

GPPrinterHelper gph = new GPPrinterHelper();
连接打印机
gph.withOpen("192.168.0.1", 9100)
  //设置打印机纸张，和边距；
   .withStep(100, 180, 1, 1)
   //打印图片
   .withDownload(@"D:\ddd.bmp", "LOGO.BMP")
   .withClear()
   .withBitmap("LOGO.BMP", 0, 0)
   .withText("TEST", 0, 25, "7")
    //打印条码
   .withBarcode("ABC123456", 0, 0, true)
   //打印中文
   .withTextCN("测试", 0, 675, 2)
   //打印二维码
   .withQrcode("asdf56", 0, 915, 4)
   //执行打印
   .Pinter();
