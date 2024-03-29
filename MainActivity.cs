﻿using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections;

namespace Xamrin_Sliding_Puzzle
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button resetButton;
        GridLayout mainLayout;

        int gameViewWidth;
        int tileWidth;

        ArrayList tilesArr;
        ArrayList coordArr;
        Point emptySpot;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            SetGameView();
            MakeTilesMethod();
            RandomizeMethod();
        }

        private void RandomizeMethod()
        {
            ArrayList tempCoords = new ArrayList(coordArr);

           Random myRand = new Random();
           foreach(MyTextView any in tilesArr)
            {
                int randIdx = myRand.Next(0,tempCoords.Count);
                Point thisRandLoc = (Point)tempCoords[randIdx];

                GridLayout.Spec rowSpec = GridLayout.InvokeSpec(thisRandLoc.Y);
                GridLayout.Spec colSpec = GridLayout.InvokeSpec(thisRandLoc.X);
                
                GridLayout.LayoutParams randLayoutParams = new GridLayout.LayoutParams(rowSpec, colSpec);

                any.xPos = thisRandLoc.X;
                any.yPos = thisRandLoc.Y;
                randLayoutParams.Width = tileWidth - 10;
                randLayoutParams.Height = tileWidth - 10;
                randLayoutParams.SetMargins(5,5,5,5);


                any.LayoutParameters = randLayoutParams;
                tempCoords.RemoveAt(randIdx);

            }
            emptySpot = (Point)tempCoords[0];
        }

        private void MakeTilesMethod()
        {
            tilesArr = new ArrayList();
            coordArr = new ArrayList();
            int counter = 1;
            for (var h = 0; h < 4; h++) { 
                for(var v = 0; v < 4; v++) { 
                    tileWidth = gameViewWidth / 4;
                    MyTextView textTile = new MyTextView(this);
                    GridLayout.Spec rowSpec = GridLayout.InvokeSpec(h);
                    GridLayout.Spec colSpec = GridLayout.InvokeSpec(v);

                    GridLayout.LayoutParams tileLayoutParams = new GridLayout.LayoutParams(rowSpec, colSpec);

                    textTile.Text = counter.ToString();
                    textTile.SetBackgroundColor(Color.Black);
                    textTile.TextSize = 40;
                    textTile.Gravity = GravityFlags.Center;

                    tileLayoutParams.Width = tileWidth - 10 ;
                    tileLayoutParams.Height = tileWidth - 10;
                    tileLayoutParams.SetMargins(5, 5, 5, 5);


                    textTile.LayoutParameters = tileLayoutParams;
                    textTile.SetBackgroundColor(Color.Green);

                    Point thisLoc = new Point(v, h);
                    coordArr.Add(thisLoc);

                    textTile.xPos = thisLoc.X;
                    textTile.yPos = thisLoc.Y;

                    textTile.Touch += TextTile_Touch_Method;

                    tilesArr.Add(textTile);


                    mainLayout.AddView(textTile);
                    counter++;
                }
            }
            mainLayout.RemoveView((TextView)tilesArr[15]);
            tilesArr.RemoveAt(15);
        }

        private void TextTile_Touch_Method(object sender, View.TouchEventArgs e)
        {
            if(e.Event.Action == MotionEventActions.Up)
            {
              
                MyTextView thisTile = (MyTextView)sender;
                float xDif = (float)Math.Pow(thisTile.xPos - emptySpot.X, 2);
                float yDif = (float)Math.Pow(thisTile.yPos - emptySpot.Y, 2);

                float dist = (float)Math.Sqrt(xDif + yDif);
                if(dist == 1)
                {
                    Point curPoint = new Point(thisTile.xPos, thisTile.yPos);
                    GridLayout.Spec rowSpec = GridLayout.InvokeSpec(emptySpot.X);
                    GridLayout.Spec colSpec = GridLayout.InvokeSpec(emptySpot.Y);
                    
                    GridLayout.LayoutParams newLocParams = new GridLayout.LayoutParams(rowSpec, colSpec);
                    thisTile.xPos = emptySpot.X;
                    thisTile.yPos = emptySpot.Y;
                    newLocParams.Width = tileWidth - 10;
                    newLocParams.Height = tileWidth - 10;
                    newLocParams.SetMargins(5, 5, 5, 5);
                    emptySpot = curPoint;
                }

            }
        }

        private void SetGameView()
        {
            resetButton = FindViewById<Button>(Resource.Id.resetButton);
            resetButton.Click += ResetMethod;

            mainLayout = FindViewById<GridLayout>(Resource.Id.gameGridLayoutID);
            gameViewWidth = Resources.DisplayMetrics.WidthPixels;

            mainLayout.ColumnCount = 4;
            mainLayout.RowCount = 4;

            mainLayout.LayoutParameters = new LinearLayout.LayoutParams(gameViewWidth, gameViewWidth);
            mainLayout.SetBackgroundColor(Color.Gray);
        }

        private void ResetMethod(object sender, System.EventArgs e)
        {
           RandomizeMethod();
        }
    }

    class MyTextView : TextView
    {
        Activity myContext;
        public MyTextView (Activity context) : base (context)
        {
            myContext = context;
        }

        public int xPos { set; get; }
        public int yPos { set; get; } 
    }
}