using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using SkiaSharp;

namespace FullService_Layout
{
    public partial class MainPage : ContentPage
    {
        public List<Table> Tables { get; set; }

        SKPaint availablePaint = new SKPaint() { Style = SKPaintStyle.Stroke, Color = SKColor.Parse("#FFFFFF") };

        SKPaint reservedPaint = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, Color = SKColor.Parse("#343352") };

        SKPaint yourSeatPaint = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, Color = SKColor.Parse("#9747FF") };

        SKPaint textPaint = new SKPaint() { TextSize = 40, Color = SKColor.Parse("#343352") };

        public MainPage()
        {
            InitializeComponent();
            GetTables();

            int columns = 10;
            int rows = 9;
            int itemWidth = 68;
            int itemHeight = 68;

            var countItems = columns * rows;
            int columnIndex = 0;
            int rowIndex = 0;

            for (int i = 0; i < countItems; i++)
            {
                int col = columnIndex++;
                int row = rowIndex;

                if (columnIndex == columns)
                {
                    columnIndex = 0;
                    rowIndex++;
                }

                Button button = new Button();

                Tables.ForEach(table =>
                {
                    int tableCol = table.PositionX / itemWidth;
                    int tableRow = table.PositionY / itemHeight;

                    int tableWidth = 68;
                    int tableHeight = 68;

                    if (tableCol == col && tableRow == row)
                    {
                        button.WidthRequest = tableWidth;
                        button.HeightRequest = tableHeight;
                        button.BorderColor = Color.Red;
                        button.BorderWidth = 5;
                        button.CornerRadius = 5;
                    }

                });

                grid_Tables.Children.Add(button, col, row);
            }

            //for(int i = 0; i < Tables.Count; i++)
            //{
            //    Table table = Tables[i];
            //    grid_Tables.Children.Add(new Button
            //    {
            //        BorderColor = Color.Blue,
            //        BorderWidth = 3,
            //        WidthRequest = table.Width / 68,
            //        HeightRequest = table.Height / 68
            //    }, table.PositionX / 10, table.PositionY / 9);
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //grid_Tables.Children.Add(new Button
            //{
            //    BorderColor = Color.Blue,
            //    BorderWidth = 3,
            //    WidthRequest = 68,
            //    HeightRequest = 68
            //}, 0, 0);

            //grid_Tables.Children.Add(new Button
            //{
            //    BorderColor = Color.Purple,
            //    BorderWidth = 3,
            //    WidthRequest = 68,
            //    HeightRequest = 64
            //}, 10, 9);

            //grid_Tables.Children.Add(new Button
            //{
            //    BorderColor = Color.Green,
            //    BorderWidth = 3,
            //    WidthRequest = 68,
            //    HeightRequest = 20
            //}, 2, 5);

            //double width = Application.Current.MainPage.Width;
            //double height = Application.Current.MainPage.Height - ( 100 + 64 );
            //double a = 0;
            //double b = 0;
            //if (width < height)
            //{

            //}
            //else if(width > height)
            //{
            //    a = width / 10;
            //    b = height / 9;

            //    a = Math.Round(a);
            //    b = Math.Round(b);

            //    var minSize = Math.Min(a,b);
            //}
        }

        private List<Table> GetTables()
        {
            try
            {
                Tables = new List<Table>();
                GetJsonData();
                return Tables;

            }
            catch (Exception ex)
            {
                return new List<Table>();
            }
        }

        private void GetJsonData()
        {
            string jsonFileName = "TableLayout.json";
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");
            using (var reader = new System.IO.StreamReader(stream))
            {
                try
                {
                    var jsonString = reader.ReadToEnd();
                    RootObject rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
                    List<Table> listTable = rootObject.Tables;
                    Tables = new List<Table>();
                    Tables.Clear();
                    foreach (Table t in listTable)
                    {
                        Tables.Add(t);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        void SKCanvasView_PaintSurface(System.Object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs args)
        {
            try
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                using (SKPaint strokePaint = new SKPaint())
                {
                    strokePaint.Color = SKColors.Red;
                    strokePaint.Style = SKPaintStyle.Stroke;
                    strokePaint.StrokeWidth = 3;

                    float x = 0;
                    float y = 0;
                    float wid = 68;
                    float hei = 68;

                    int column = 10;
                    int row = 9;

                    int perCol = (info.Width / column);
                    int perRow = info.Height / row;

                    for (int i = 0; i < Tables.Count; i++)
                    {
                        //if(i == 0)
                        //{
                        //    x += 60;
                        //}

                        Table table = Tables[i];
                        x = (float)table.PositionX + perCol;
                        y = (float)table.PositionY + perRow;
                        wid = (float)table.Width;
                        hei = (float)table.Height;

                        SKRect rect = new SKRect(x, y, x + wid, y + hei);

                        canvas.DrawRect(rect, strokePaint);
                        
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        SKPaint GetColor(int seatColor)
        {
            switch (seatColor)
            {
                case 0:
                default:
                    return availablePaint;
                case 1:
                    return reservedPaint;
                case 2:
                    return yourSeatPaint;
            }
        }

        void grid_Tables_BindingContextChanged(System.Object sender, System.EventArgs e)
        {
            try
            {
                
            }
            catch(Exception ex)
            {

            }
        }
    }

    public class Table
    {
        public string TableID { get; set; }
        public string TableCode { get; set; }
        public string AreaID { get; set; }
        public string TableName { get; set; }
        public string TableNameNoPrefix { get; set; }
        public string Description { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Picture { get; set; }
        public int Seat { get; set; }
        public bool IsEnable { get; set; }
        public int Status { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string DurationTime { get; set; }
        public string CustomerID { get; set; }
        public string picture { get; set; }
    }

    public class RootObject
    {
        public List<Table> Tables { get; set; }
    }
}
