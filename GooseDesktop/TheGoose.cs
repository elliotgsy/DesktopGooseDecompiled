// Decompiled with JetBrains decompiler
// Type: GooseDesktop.TheGoose
// Assembly: GooseDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 163C7FB0-432D-4C0F-A75F-86497CB58900
// Assembly location: C:\Users\9613elliotb\Desktop\Desktop Goose v0.21\GooseDesktop.exe

using SamEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GooseDesktop
{
  internal static class TheGoose
  {
    private static Vector2 position = new Vector2(300f, 300f);
    private static Vector2 velocity = new Vector2(0.0f, 0.0f);
    private static float direction = 90f;
    private static Vector2 targetPos = new Vector2(300f, 300f);
    private static float targetDir = 90f;
    private static float currentSpeed = 80f;
    private static float currentAcceleration = 1300f;
    private static float stepTime = 0.2f;
    private static float trackMudEndTime = -1f;
    private static FootMark[] footMarks = new FootMark[64];
    private static int footMarkIndex = 0;
    private static bool lastFrameMouseButtonPressed = false;
    private static Rectangle tmpRect = new Rectangle();
    private static Size tmpSize = new Size();
    private static bool hasAskedForDonation = false;
    private static TheGoose.GooseTask[] gooseTaskWeightedList = new TheGoose.GooseTask[8]
    {
      TheGoose.GooseTask.TrackMud,
      TheGoose.GooseTask.TrackMud,
      TheGoose.GooseTask.CollectWindow_Meme,
      TheGoose.GooseTask.CollectWindow_Meme,
      TheGoose.GooseTask.CollectWindow_Notepad,
      TheGoose.GooseTask.NabMouse,
      TheGoose.GooseTask.NabMouse,
      TheGoose.GooseTask.NabMouse
    };
    private static Deck taskPickerDeck = new Deck(TheGoose.gooseTaskWeightedList.Length);
    private static float lFootMoveTimeStart = -1f;
    private static float rFootMoveTimeStart = -1f;
    private static Vector2 targetDirection;
    private static bool overrideExtendNeck;
    private const TheGoose.GooseTask FirstUX_FirstTask = TheGoose.GooseTask.TrackMud;
    private const TheGoose.GooseTask FirstUX_SecondTask = TheGoose.GooseTask.CollectWindow_Meme;
    private const float WalkSpeed = 80f;
    private const float RunSpeed = 200f;
    private const float ChargeSpeed = 400f;
    private const float turnSpeed = 120f;
    private const float AccelerationNormal = 1300f;
    private const float AccelerationCharged = 2300f;
    private const float StopRadius = -10f;
    private const float StepTimeNormal = 0.2f;
    private const float StepTimeCharged = 0.1f;
    private const float DurationToTrackMud = 15f;
    private static Pen DrawingPen;
    private static Bitmap shadowBitmap;
    private static TextureBrush shadowBrush;
    private static Pen shadowPen;
    private static TheGoose.GooseTask currentTask;
    private static TheGoose.Task_Wander taskWanderInfo;
    private static TheGoose.Task_NabMouse taskNabMouseInfo;
    private static TheGoose.Task_CollectWindow taskCollectWindowInfo;
    private static TheGoose.Task_TrackMud taskTrackMudInfo;
    private static Vector2 lFootPos;
    private static Vector2 rFootPos;
    private static Vector2 lFootMoveOrigin;
    private static Vector2 rFootMoveOrigin;
    private static Vector2 lFootMoveDir;
    private static Vector2 rFootMoveDir;
    private const float wantStepAtDistance = 5f;
    private const int feetDistanceApart = 6;
    private const float overshootFraction = 0.4f;
    private static TheGoose.Rig gooseRig;

    public static void Init()
    {
      TheGoose.position = new Vector2(-20f, 120f);
      TheGoose.targetPos = new Vector2(100f, 150f);
      if (!GooseConfig.settings.CanAttackAtRandom)
      {
        int index1 = Array.IndexOf<int>(TheGoose.taskPickerDeck.indices, Array.IndexOf<TheGoose.GooseTask>(TheGoose.gooseTaskWeightedList, TheGoose.GooseTask.CollectWindow_Meme));
        int index2 = TheGoose.taskPickerDeck.indices[0];
        TheGoose.taskPickerDeck.indices[0] = TheGoose.taskPickerDeck.indices[index1];
        TheGoose.taskPickerDeck.indices[index1] = index2;
      }
      TheGoose.lFootPos = TheGoose.GetFootHome(false);
      TheGoose.rFootPos = TheGoose.GetFootHome(true);
      TheGoose.shadowBitmap = new Bitmap(2, 2);
      TheGoose.shadowBitmap.SetPixel(0, 0, Color.Transparent);
      TheGoose.shadowBitmap.SetPixel(1, 1, Color.Transparent);
      TheGoose.shadowBitmap.SetPixel(1, 0, Color.Transparent);
      TheGoose.shadowBitmap.SetPixel(0, 1, Color.DarkGray);
      TheGoose.shadowBrush = new TextureBrush((Image) TheGoose.shadowBitmap);
      TheGoose.shadowPen = new Pen((Brush) TheGoose.shadowBrush);
      TheGoose.shadowPen.StartCap = TheGoose.shadowPen.EndCap = LineCap.Round;
      TheGoose.DrawingPen = new Pen(Brushes.White);
      TheGoose.DrawingPen.EndCap = TheGoose.DrawingPen.StartCap = LineCap.Round;
      TheGoose.SetTask(TheGoose.GooseTask.Wander);
    }

    private static void SetSpeed(TheGoose.SpeedTiers tier)
    {
      float speedMod = 1f;

      switch (tier)
      {
        case TheGoose.SpeedTiers.Walk:
          TheGoose.currentSpeed = 80f * speedMod;
          TheGoose.currentAcceleration = 1300f * speedMod;
          TheGoose.stepTime = 0.2f;
          break;
        case TheGoose.SpeedTiers.Run:
          TheGoose.currentSpeed = 200f * speedMod;
          TheGoose.currentAcceleration = 1300f * speedMod;
          TheGoose.stepTime = 0.2f;
          break;
        case TheGoose.SpeedTiers.Charge:
          TheGoose.currentSpeed = 400f * speedMod;
          TheGoose.currentAcceleration = 2300f * speedMod;
          TheGoose.stepTime = 0.1f;
          break;
      }
    }

    public static void Tick()
    {
      Cursor.Clip = Rectangle.Empty;
      if (TheGoose.currentTask != TheGoose.GooseTask.NabMouse && (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && !TheGoose.lastFrameMouseButtonPressed && (double) Vector2.Distance(TheGoose.position + new Vector2(0.0f, 14f), new Vector2((float) Cursor.Position.X, (float) Cursor.Position.Y)) < 30.0)
        TheGoose.SetTask(TheGoose.GooseTask.NabMouse);
      TheGoose.lastFrameMouseButtonPressed = (Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left;
      TheGoose.targetDirection = Vector2.Normalize(TheGoose.targetPos - TheGoose.position);
      TheGoose.overrideExtendNeck = false;
      TheGoose.RunAI();
      Vector2 vector2 = Vector2.Lerp(Vector2.GetFromAngleDegrees(TheGoose.direction), TheGoose.targetDirection, 0.25f);
      TheGoose.direction = (float) Math.Atan2((double) vector2.y, (double) vector2.x) * 57.29578f;
      if ((double) Vector2.Magnitude(TheGoose.velocity) > (double) TheGoose.currentSpeed)
        TheGoose.velocity = Vector2.Normalize(TheGoose.velocity) * TheGoose.currentSpeed;
      TheGoose.velocity += Vector2.Normalize(TheGoose.targetPos - TheGoose.position) * TheGoose.currentAcceleration * 0.008333334f;
      TheGoose.position += TheGoose.velocity * 0.008333334f;
      TheGoose.SolveFeet();
      double num1 = (double) Vector2.Magnitude(TheGoose.velocity);
      int num2 = TheGoose.overrideExtendNeck | (double) TheGoose.currentSpeed >= 200.0 ? 1 : 0;
      TheGoose.gooseRig.neckLerpPercent = SamMath.Lerp(TheGoose.gooseRig.neckLerpPercent, (float) num2, 0.075f);
    }

    private static void RunWander()
    {
      if ((double) Time.time - (double) TheGoose.taskWanderInfo.wanderingStartTime > (double) TheGoose.taskWanderInfo.wanderingDuration)
        TheGoose.ChooseNextTask();
      else if ((double) TheGoose.taskWanderInfo.pauseStartTime > 0.0)
      {
        if ((double) Time.time - (double) TheGoose.taskWanderInfo.pauseStartTime > (double) TheGoose.taskWanderInfo.pauseDuration)
        {
          TheGoose.taskWanderInfo.pauseStartTime = -1f;
          float num = TheGoose.Task_Wander.GetRandomWalkTime() * TheGoose.currentSpeed;
          TheGoose.targetPos = new Vector2(SamMath.RandomRange(0.0f, (float) Program.mainForm.Width), SamMath.RandomRange(0.0f, (float) Program.mainForm.Height));
          if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) <= (double) num)
            return;
          TheGoose.targetPos = TheGoose.position + Vector2.Normalize(TheGoose.targetPos - TheGoose.position) * num;
        }
        else
          TheGoose.velocity = Vector2.zero;
      }
      else
      {
        if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 20.0)
          return;
        TheGoose.taskWanderInfo.pauseStartTime = Time.time;
        TheGoose.taskWanderInfo.pauseDuration = TheGoose.Task_Wander.GetRandomPauseDuration();
      }
    }

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    private static void RunNabMouse()
    {
      Vector2 b = Vector2.zero;
      ref Vector2 local = ref b;
      Point position = Cursor.Position;
      double x = (double) position.X;
      position = Cursor.Position;
      double y = (double) position.Y;
      local = new Vector2((float) x, (float) y);
      Vector2 head2EndPoint = TheGoose.gooseRig.head2EndPoint;
      if (TheGoose.taskNabMouseInfo.currentStage == TheGoose.Task_NabMouse.Stage.SeekingMouse)
      {
        TheGoose.SetSpeed(TheGoose.SpeedTiers.Charge);
        TheGoose.targetPos = b - (TheGoose.gooseRig.head2EndPoint - TheGoose.position);
        if ((double) Vector2.Distance(head2EndPoint, b) < 15.0)
        {
          TheGoose.taskNabMouseInfo.originalVectorToMouse = b - head2EndPoint;
          TheGoose.taskNabMouseInfo.grabbedOriginalTime = Time.time;
          TheGoose.taskNabMouseInfo.dragToPoint = TheGoose.position;
          while ((double) Vector2.Distance(TheGoose.taskNabMouseInfo.dragToPoint, TheGoose.position) / 400.0 < 1.20000004768372)
            TheGoose.taskNabMouseInfo.dragToPoint = new Vector2((float) SamMath.Rand.NextDouble() * (float) Program.mainForm.Width, (float) SamMath.Rand.NextDouble() * (float) Program.mainForm.Height);
          TheGoose.targetPos = TheGoose.taskNabMouseInfo.dragToPoint;
          TheGoose.SetForegroundWindow(Program.mainForm.Handle);
          Sound.CHOMP();
          TheGoose.taskNabMouseInfo.currentStage = TheGoose.Task_NabMouse.Stage.DraggingMouseAway;
        }
        if ((double) Time.time > (double) TheGoose.taskNabMouseInfo.chaseStartTime + 9.0)
          TheGoose.taskNabMouseInfo.currentStage = TheGoose.Task_NabMouse.Stage.Decelerating;
      }
      if (TheGoose.taskNabMouseInfo.currentStage == TheGoose.Task_NabMouse.Stage.DraggingMouseAway)
      {
        if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) < 30.0)
        {
          Cursor.Clip = Rectangle.Empty;
          TheGoose.taskNabMouseInfo.currentStage = TheGoose.Task_NabMouse.Stage.Decelerating;
        }
        else
        {
          float p = Math.Min((float) (((double) Time.time - (double) TheGoose.taskNabMouseInfo.grabbedOriginalTime) / 0.0599999986588955), 1f);
          Vector2 vector2 = Vector2.Lerp(TheGoose.taskNabMouseInfo.originalVectorToMouse, TheGoose.Task_NabMouse.StruggleRange, p);
          TheGoose.tmpRect.Location = TheGoose.ToIntPoint(new Vector2()
          {
            x = (double) vector2.x < 0.0 ? head2EndPoint.x + vector2.x : head2EndPoint.x,
            y = (double) vector2.y < 0.0 ? head2EndPoint.y + vector2.y : head2EndPoint.y
          });
          TheGoose.tmpSize.Width = Math.Abs((int) vector2.x);
          TheGoose.tmpSize.Height = Math.Abs((int) vector2.y);
          TheGoose.tmpRect.Size = TheGoose.tmpSize;
          Cursor.Clip = TheGoose.tmpRect;
        }
      }
      if (TheGoose.taskNabMouseInfo.currentStage != TheGoose.Task_NabMouse.Stage.Decelerating)
        return;
      TheGoose.targetPos = TheGoose.position + Vector2.Normalize(TheGoose.velocity) * 5f;
      TheGoose.velocity -= Vector2.Normalize(TheGoose.velocity) * TheGoose.currentAcceleration * 2f * 0.008333334f;
      if ((double) Vector2.Magnitude(TheGoose.velocity) >= 80.0)
        return;
      TheGoose.SetTask(TheGoose.GooseTask.Wander);
    }

    private static void RunCollectWindow()
    {
      switch (TheGoose.taskCollectWindowInfo.stage)
      {
        case TheGoose.Task_CollectWindow.Stage.WalkingOffscreen:
          if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 5.0)
            break;
          TheGoose.taskCollectWindowInfo.secsToWait = TheGoose.Task_CollectWindow.GetWaitTime();
          TheGoose.taskCollectWindowInfo.waitStartTime = Time.time;
          TheGoose.taskCollectWindowInfo.stage = TheGoose.Task_CollectWindow.Stage.WaitingToBringWindowBack;
          break;
        case TheGoose.Task_CollectWindow.Stage.WaitingToBringWindowBack:
          if ((double) Time.time - (double) TheGoose.taskCollectWindowInfo.waitStartTime <= (double) TheGoose.taskCollectWindowInfo.secsToWait)
            break;
          TheGoose.taskCollectWindowInfo.mainForm.FormClosing += new FormClosingEventHandler(TheGoose.CollectMemeTask_CancelEarly);
          int num;
          new Thread((ThreadStart) (() => num = (int) TheGoose.taskCollectWindowInfo.mainForm.ShowDialog())).Start();
          switch (TheGoose.taskCollectWindowInfo.screenDirection)
          {
            case TheGoose.Task_CollectWindow.ScreenDirection.Left:
              TheGoose.targetPos.y = SamMath.Lerp(TheGoose.position.y, (float) (Program.mainForm.Height / 2), SamMath.RandomRange(0.2f, 0.3f));
              TheGoose.targetPos.x = (float) TheGoose.taskCollectWindowInfo.mainForm.Width + SamMath.RandomRange(15f, 20f);
              break;
            case TheGoose.Task_CollectWindow.ScreenDirection.Top:
              TheGoose.targetPos.y = (float) TheGoose.taskCollectWindowInfo.mainForm.Height + SamMath.RandomRange(80f, 100f);
              TheGoose.targetPos.x = SamMath.Lerp(TheGoose.position.x, (float) (Program.mainForm.Width / 2), SamMath.RandomRange(0.2f, 0.3f));
              break;
            case TheGoose.Task_CollectWindow.ScreenDirection.Right:
              TheGoose.targetPos.y = SamMath.Lerp(TheGoose.position.y, (float) (Program.mainForm.Height / 2), SamMath.RandomRange(0.2f, 0.3f));
              TheGoose.targetPos.x = (float) Program.mainForm.Width - ((float) TheGoose.taskCollectWindowInfo.mainForm.Width + SamMath.RandomRange(20f, 30f));
              break;
          }
          TheGoose.targetPos.x = SamMath.Clamp(TheGoose.targetPos.x, (float) (TheGoose.taskCollectWindowInfo.mainForm.Width + 55), (float) (Program.mainForm.Width - (TheGoose.taskCollectWindowInfo.mainForm.Width + 55)));
          TheGoose.targetPos.y = SamMath.Clamp(TheGoose.targetPos.y, (float) (TheGoose.taskCollectWindowInfo.mainForm.Height + 80), (float) Program.mainForm.Height);
          TheGoose.taskCollectWindowInfo.stage = TheGoose.Task_CollectWindow.Stage.DraggingWindowBack;
          break;
        case TheGoose.Task_CollectWindow.Stage.DraggingWindowBack:
          if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) < 5.0)
          {
            TheGoose.targetPos = TheGoose.position + Vector2.GetFromAngleDegrees(TheGoose.direction + 180f) * 40f;
            TheGoose.SetTask(TheGoose.GooseTask.Wander);
            break;
          }
          TheGoose.overrideExtendNeck = true;
          TheGoose.targetDirection = TheGoose.position - TheGoose.targetPos;
          TheGoose.taskCollectWindowInfo.mainForm.SetWindowPositionThreadsafe(TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint - TheGoose.taskCollectWindowInfo.windowOffsetToBeak));
          break;
      }
    }

    private static void CollectMemeTask_CancelEarly(object sender, FormClosingEventArgs args)
    {
      TheGoose.SetTask(TheGoose.GooseTask.NabMouse);
    }

    private static void RunTrackMud()
    {
      switch (TheGoose.taskTrackMudInfo.stage)
      {
        case TheGoose.Task_TrackMud.Stage.DecideToRun:
          int num = (int) TheGoose.SetTargetOffscreen(false);
          TheGoose.SetSpeed(TheGoose.SpeedTiers.Run);
          TheGoose.taskTrackMudInfo.stage = TheGoose.Task_TrackMud.Stage.RunningOffscreen;
          break;
        case TheGoose.Task_TrackMud.Stage.RunningOffscreen:
          if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) >= 5.0)
            break;
          TheGoose.targetPos = new Vector2(SamMath.RandomRange(0.0f, (float) Program.mainForm.Width), SamMath.RandomRange(0.0f, (float) Program.mainForm.Height));
          TheGoose.taskTrackMudInfo.nextDirChangeTime = Time.time + TheGoose.Task_TrackMud.GetDirChangeInterval();
          TheGoose.taskTrackMudInfo.timeToStopRunning = Time.time + 2f;
          TheGoose.trackMudEndTime = Time.time + 15f;
          TheGoose.taskTrackMudInfo.stage = TheGoose.Task_TrackMud.Stage.RunningWandering;
          Sound.PlayMudSquith();
          break;
        case TheGoose.Task_TrackMud.Stage.RunningWandering:
          if ((double) Vector2.Distance(TheGoose.position, TheGoose.targetPos) < 5.0 || (double) Time.time > (double) TheGoose.taskTrackMudInfo.nextDirChangeTime)
          {
            TheGoose.targetPos = new Vector2(SamMath.RandomRange(0.0f, (float) Program.mainForm.Width), SamMath.RandomRange(0.0f, (float) Program.mainForm.Height));
            TheGoose.taskTrackMudInfo.nextDirChangeTime = Time.time + TheGoose.Task_TrackMud.GetDirChangeInterval();
          }
          if ((double) Time.time <= (double) TheGoose.taskTrackMudInfo.timeToStopRunning)
            break;
          TheGoose.targetPos = TheGoose.position + new Vector2(30f, 3f);
          TheGoose.targetPos.x = SamMath.Clamp(TheGoose.targetPos.x, 55f, (float) (Program.mainForm.Width - 55));
          TheGoose.targetPos.y = SamMath.Clamp(TheGoose.targetPos.y, 80f, (float) (Program.mainForm.Height - 80));
          TheGoose.SetTask(TheGoose.GooseTask.Wander, false);
          break;
      }
    }

    private static void ChooseNextTask()
    {
      if (!GooseConfig.settings.CanAttackAtRandom && (double) Time.time < (double) GooseConfig.settings.FirstWanderTimeSeconds + 1.0)
      {
        TheGoose.SetTask(TheGoose.GooseTask.TrackMud);
      }
      else
      {
        float num = 8f;
        if ((double) Time.time > (double) num * 60.0 && !TheGoose.hasAskedForDonation)
        {
          TheGoose.hasAskedForDonation = true;
          TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_Donate);
        }
        else
        {
          TheGoose.GooseTask gooseTaskWeighted = TheGoose.gooseTaskWeightedList[TheGoose.taskPickerDeck.Next()];
          while (!GooseConfig.settings.CanAttackAtRandom && gooseTaskWeighted == TheGoose.GooseTask.NabMouse)
            gooseTaskWeighted = TheGoose.gooseTaskWeightedList[TheGoose.taskPickerDeck.Next()];
          TheGoose.SetTask(gooseTaskWeighted);
        }
      }
    }

    private static void SetTask(TheGoose.GooseTask task)
    {
      TheGoose.SetTask(task, true);
    }

    private static void SetTask(TheGoose.GooseTask task, bool honck)
    {
      if (honck)
        Sound.HONCC();
      TheGoose.currentTask = task;
      switch (task)
      {
        case TheGoose.GooseTask.Wander:
          TheGoose.SetSpeed(TheGoose.SpeedTiers.Walk);
          TheGoose.taskWanderInfo = new TheGoose.Task_Wander();
          TheGoose.taskWanderInfo.pauseStartTime = -1f;
          TheGoose.taskWanderInfo.wanderingStartTime = Time.time;
          TheGoose.taskWanderInfo.wanderingDuration = TheGoose.Task_Wander.GetRandomWanderDuration();
          break;
        case TheGoose.GooseTask.NabMouse:
          TheGoose.taskNabMouseInfo = new TheGoose.Task_NabMouse();
          TheGoose.taskNabMouseInfo.chaseStartTime = Time.time;
          break;
        case TheGoose.GooseTask.CollectWindow_Meme:
          TheGoose.taskCollectWindowInfo = new TheGoose.Task_CollectWindow();
          TheGoose.taskCollectWindowInfo.mainForm = (TheGoose.MovableForm) new TheGoose.SimpleImageForm();
          TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_DONOTSET, false);
          break;
        case TheGoose.GooseTask.CollectWindow_Notepad:
          TheGoose.taskCollectWindowInfo = new TheGoose.Task_CollectWindow();
          TheGoose.taskCollectWindowInfo.mainForm = (TheGoose.MovableForm) new TheGoose.SimpleTextForm();
          TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_DONOTSET, false);
          break;
        case TheGoose.GooseTask.CollectWindow_Donate:
          TheGoose.taskCollectWindowInfo = new TheGoose.Task_CollectWindow();
          TheGoose.taskCollectWindowInfo.mainForm = (TheGoose.MovableForm) new TheGoose.SimpleDonateForm();
          TheGoose.SetTask(TheGoose.GooseTask.CollectWindow_DONOTSET, false);
          break;
        case TheGoose.GooseTask.CollectWindow_DONOTSET:
          TheGoose.taskCollectWindowInfo.screenDirection = TheGoose.SetTargetOffscreen(false);
          switch (TheGoose.taskCollectWindowInfo.screenDirection)
          {
            case TheGoose.Task_CollectWindow.ScreenDirection.Left:
              TheGoose.taskCollectWindowInfo.windowOffsetToBeak = new Vector2((float) TheGoose.taskCollectWindowInfo.mainForm.Width, (float) (TheGoose.taskCollectWindowInfo.mainForm.Height / 2));
              return;
            case TheGoose.Task_CollectWindow.ScreenDirection.Top:
              TheGoose.taskCollectWindowInfo.windowOffsetToBeak = new Vector2((float) (TheGoose.taskCollectWindowInfo.mainForm.Width / 2), (float) TheGoose.taskCollectWindowInfo.mainForm.Height);
              return;
            case TheGoose.Task_CollectWindow.ScreenDirection.Right:
              TheGoose.taskCollectWindowInfo.windowOffsetToBeak = new Vector2(0.0f, (float) (TheGoose.taskCollectWindowInfo.mainForm.Height / 2));
              return;
            default:
              return;
          }
        case TheGoose.GooseTask.TrackMud:
          TheGoose.taskTrackMudInfo = new TheGoose.Task_TrackMud();
          break;
      }
    }

    private static void RunAI()
    {
      switch (TheGoose.currentTask)
      {
        case TheGoose.GooseTask.Wander:
          TheGoose.RunWander();
          break;
        case TheGoose.GooseTask.NabMouse:
          TheGoose.RunNabMouse();
          break;
        case TheGoose.GooseTask.CollectWindow_DONOTSET:
          TheGoose.RunCollectWindow();
          break;
        case TheGoose.GooseTask.TrackMud:
          TheGoose.RunTrackMud();
          break;
      }
    }

    private static TheGoose.Task_CollectWindow.ScreenDirection SetTargetOffscreen(
      bool canExitTop = false)
    {
      int num = (int) TheGoose.position.x;
      TheGoose.Task_CollectWindow.ScreenDirection screenDirection = TheGoose.Task_CollectWindow.ScreenDirection.Left;
      TheGoose.targetPos = new Vector2(-50f, SamMath.Lerp(TheGoose.position.y, (float) (Program.mainForm.Height / 2), 0.4f));
      if (num > Program.mainForm.Width / 2)
      {
        num = Program.mainForm.Width - (int) TheGoose.position.x;
        screenDirection = TheGoose.Task_CollectWindow.ScreenDirection.Right;
        TheGoose.targetPos = new Vector2((float) (Program.mainForm.Width + 50), SamMath.Lerp(TheGoose.position.y, (float) (Program.mainForm.Height / 2), 0.4f));
      }
      if (canExitTop && (double) num > (double) TheGoose.position.y)
      {
        screenDirection = TheGoose.Task_CollectWindow.ScreenDirection.Top;
        TheGoose.targetPos = new Vector2(SamMath.Lerp(TheGoose.position.x, (float) (Program.mainForm.Width / 2), 0.4f), -50f);
      }
      return screenDirection;
    }

    private static void SolveFeet()
    {
      Vector2.GetFromAngleDegrees(TheGoose.direction);
      Vector2.GetFromAngleDegrees(TheGoose.direction + 90f);
      Vector2 footHome1 = TheGoose.GetFootHome(false);
      Vector2 footHome2 = TheGoose.GetFootHome(true);
      if ((double) TheGoose.lFootMoveTimeStart < 0.0 && (double) TheGoose.rFootMoveTimeStart < 0.0)
      {
        if ((double) Vector2.Distance(TheGoose.lFootPos, footHome1) > 5.0)
        {
          TheGoose.lFootMoveOrigin = TheGoose.lFootPos;
          TheGoose.lFootMoveDir = Vector2.Normalize(footHome1 - TheGoose.lFootPos);
          TheGoose.lFootMoveTimeStart = Time.time;
        }
        else
        {
          if ((double) Vector2.Distance(TheGoose.rFootPos, footHome2) <= 5.0)
            return;
          TheGoose.rFootMoveOrigin = TheGoose.rFootPos;
          TheGoose.rFootMoveDir = Vector2.Normalize(footHome2 - TheGoose.rFootPos);
          TheGoose.rFootMoveTimeStart = Time.time;
        }
      }
      else if ((double) TheGoose.lFootMoveTimeStart > 0.0)
      {
        Vector2 b = footHome1 + TheGoose.lFootMoveDir * 0.4f * 5f;
        if ((double) Time.time > (double) TheGoose.lFootMoveTimeStart + (double) TheGoose.stepTime)
        {
          TheGoose.lFootPos = b;
          TheGoose.lFootMoveTimeStart = -1f;
          Sound.PlayPat();
          if ((double) Time.time >= (double) TheGoose.trackMudEndTime)
            return;
          TheGoose.AddFootMark(TheGoose.lFootPos);
        }
        else
        {
          float p = (Time.time - TheGoose.lFootMoveTimeStart) / TheGoose.stepTime;
          TheGoose.lFootPos = Vector2.Lerp(TheGoose.lFootMoveOrigin, b, Easings.CubicEaseInOut(p));
        }
      }
      else
      {
        if ((double) TheGoose.rFootMoveTimeStart <= 0.0)
          return;
        Vector2 b = footHome2 + TheGoose.rFootMoveDir * 0.4f * 5f;
        if ((double) Time.time > (double) TheGoose.rFootMoveTimeStart + (double) TheGoose.stepTime)
        {
          TheGoose.rFootPos = b;
          TheGoose.rFootMoveTimeStart = -1f;
          Sound.PlayPat();
          if ((double) Time.time >= (double) TheGoose.trackMudEndTime)
            return;
          TheGoose.AddFootMark(TheGoose.rFootPos);
        }
        else
        {
          float p = (Time.time - TheGoose.rFootMoveTimeStart) / TheGoose.stepTime;
          TheGoose.rFootPos = Vector2.Lerp(TheGoose.rFootMoveOrigin, b, Easings.CubicEaseInOut(p));
        }
      }
    }

    private static Vector2 GetFootHome(bool rightFoot)
    {
      float num = rightFoot ? 1f : 0.0f;
      Vector2 vector2 = Vector2.GetFromAngleDegrees(TheGoose.direction + 90f) * num;
      return TheGoose.position + vector2 * 6f;
    }

    private static void AddFootMark(Vector2 markPos)
    {
      TheGoose.footMarks[TheGoose.footMarkIndex].time = Time.time;
      TheGoose.footMarks[TheGoose.footMarkIndex].position = markPos;
      ++TheGoose.footMarkIndex;
      if (TheGoose.footMarkIndex < TheGoose.footMarks.Length)
        return;
      TheGoose.footMarkIndex = 0;
    }

    public static void UpdateRig()
    {
      double direction = (double) TheGoose.direction;
      Vector2 vector2_1 = new Vector2((float) (int) TheGoose.position.x, (float) (int) TheGoose.position.y);
      Vector2 vector2_2 = new Vector2(1.3f, 0.4f);
      Vector2 fromAngleDegrees = Vector2.GetFromAngleDegrees((float) direction);
      Vector2 vector2_3 = fromAngleDegrees * vector2_2;
      Vector2 vector2_4 = Vector2.GetFromAngleDegrees((float) (direction + 90.0)) * vector2_2;
      Vector2 vector2_5 = new Vector2(0.0f, -1f);
      TheGoose.gooseRig.underbodyCenter = vector2_1 + vector2_5 * 9f;
      TheGoose.gooseRig.bodyCenter = vector2_1 + vector2_5 * 14f;
      int num1 = (int) SamMath.Lerp(20f, 10f, TheGoose.gooseRig.neckLerpPercent);
      int num2 = (int) SamMath.Lerp(3f, 16f, TheGoose.gooseRig.neckLerpPercent);
      TheGoose.gooseRig.neckCenter = vector2_1 + vector2_5 * (float) (14 + num1);
      TheGoose.gooseRig.neckBase = TheGoose.gooseRig.bodyCenter + fromAngleDegrees * 15f;
      TheGoose.gooseRig.neckHeadPoint = TheGoose.gooseRig.neckBase + fromAngleDegrees * (float) num2 + vector2_5 * (float) num1;
      TheGoose.gooseRig.head1EndPoint = TheGoose.gooseRig.neckHeadPoint + fromAngleDegrees * 3f - vector2_5 * 1f;
      TheGoose.gooseRig.head2EndPoint = TheGoose.gooseRig.head1EndPoint + fromAngleDegrees * 5f;
    }

    public static void Render(Graphics g)
    {
      for (int index = 0; index < TheGoose.footMarks.Length; ++index)
      {
        if ((double) TheGoose.footMarks[index].time != 0.0)
        {
          float num1 = TheGoose.footMarks[index].time + 8.5f;
          float num2 = SamMath.Lerp(3f, 0.0f, SamMath.Clamp(Time.time - num1, 0.0f, 1f) / 1f);
          TheGoose.FillCircleFromCenter(g, Brushes.SaddleBrown, TheGoose.footMarks[index].position, (int) num2);
        }
      }
      TheGoose.UpdateRig();
      double direction = (double) TheGoose.direction;
      Vector2 vector2_1 = new Vector2((float) (int) TheGoose.position.x, (float) (int) TheGoose.position.y);
      Vector2 vector2_2 = new Vector2(1.3f, 0.4f);
      Vector2 fromAngleDegrees1 = Vector2.GetFromAngleDegrees((float) direction);
      Vector2 vector2_3 = fromAngleDegrees1 * vector2_2;
      Vector2 fromAngleDegrees2 = Vector2.GetFromAngleDegrees((float) (direction + 90.0));
      Vector2 vector2_4 = fromAngleDegrees2 * vector2_2;
      Vector2 vector2_5 = new Vector2(0.0f, -1f);
      int num3 = 2;
      TheGoose.DrawingPen.Brush = Brushes.White;
      TheGoose.FillCircleFromCenter(g, Brushes.Orange, TheGoose.lFootPos, 4);
      TheGoose.FillCircleFromCenter(g, Brushes.Orange, TheGoose.rFootPos, 4);
      TheGoose.FillEllipseFromCenter(g, (Brush) TheGoose.shadowBrush, (int) vector2_1.x, (int) vector2_1.y, 20, 15);
      TheGoose.DrawingPen.Color = Color.LightGray;
      TheGoose.DrawingPen.Width = (float) (22 + num3);
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter + fromAngleDegrees1 * 11f), TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter - fromAngleDegrees1 * 11f));
      TheGoose.DrawingPen.Width = (float) (13 + num3);
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckBase), TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint));
      TheGoose.DrawingPen.Width = (float) (15 + num3);
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint));
      TheGoose.DrawingPen.Width = (float) (10 + num3);
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint));
      TheGoose.DrawingPen.Color = Color.LightGray;
      TheGoose.DrawingPen.Width = 15f;
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.underbodyCenter + fromAngleDegrees1 * 7f), TheGoose.ToIntPoint(TheGoose.gooseRig.underbodyCenter - fromAngleDegrees1 * 7f));
      TheGoose.DrawingPen.Color = Color.White;
      TheGoose.DrawingPen.Width = 22f;
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter + fromAngleDegrees1 * 11f), TheGoose.ToIntPoint(TheGoose.gooseRig.bodyCenter - fromAngleDegrees1 * 11f));
      TheGoose.DrawingPen.Width = 13f;
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckBase), TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint));
      TheGoose.DrawingPen.Width = 15f;
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.neckHeadPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint));
      TheGoose.DrawingPen.Width = 10f;
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.head1EndPoint), TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint));
      int num4 = 9;
      int num5 = 3;
      TheGoose.DrawingPen.Width = (float) num4;
      TheGoose.DrawingPen.Brush = Brushes.Orange;
      Vector2 vector = TheGoose.gooseRig.head2EndPoint + fromAngleDegrees1 * (float) num5;
      g.DrawLine(TheGoose.DrawingPen, TheGoose.ToIntPoint(TheGoose.gooseRig.head2EndPoint), TheGoose.ToIntPoint(vector));
      Vector2 pos1 = TheGoose.gooseRig.neckHeadPoint + vector2_5 * 3f + -fromAngleDegrees2 * vector2_2 * 5f + fromAngleDegrees1 * 5f;
      Vector2 pos2 = TheGoose.gooseRig.neckHeadPoint + vector2_5 * 3f + fromAngleDegrees2 * vector2_2 * 5f + fromAngleDegrees1 * 5f;
      TheGoose.FillCircleFromCenter(g, Brushes.Black, pos1, 2);
      TheGoose.FillCircleFromCenter(g, Brushes.Black, pos2, 2);
    }

    public static void FillCircleFromCenter(Graphics g, Brush brush, Vector2 pos, int radius)
    {
      TheGoose.FillEllipseFromCenter(g, brush, (int) pos.x, (int) pos.y, radius, radius);
    }

    public static void FillCircleFromCenter(Graphics g, Brush brush, int x, int y, int radius)
    {
      TheGoose.FillEllipseFromCenter(g, brush, x, y, radius, radius);
    }

    public static void FillEllipseFromCenter(
      Graphics g,
      Brush brush,
      int x,
      int y,
      int xRadius,
      int yRadius)
    {
      g.FillEllipse(brush, x - xRadius, y - yRadius, xRadius * 2, yRadius * 2);
    }

    public static void FillEllipseFromCenter(
      Graphics g,
      Brush brush,
      Vector2 position,
      Vector2 xyRadius)
    {
      g.FillEllipse(brush, position.x - xyRadius.x, position.y - xyRadius.y, xyRadius.x * 2f, xyRadius.y * 2f);
    }

    private static Point ToIntPoint(Vector2 vector)
    {
      return new Point((int) vector.x, (int) vector.y);
    }

    private enum SpeedTiers
    {
      Walk,
      Run,
      Charge,
    }

    private enum GooseTask
    {
      Wander,
      NabMouse,
      CollectWindow_Meme,
      CollectWindow_Notepad,
      CollectWindow_Donate,
      CollectWindow_DONOTSET,
      TrackMud,
      Count,
    }

    private struct Task_Wander
    {
      private const float MinPauseTime = 1f;
      private const float MaxPauseTime = 2f;
      public const float GoodEnoughDistance = 20f;
      public float wanderingStartTime;
      public float wanderingDuration;
      public float pauseStartTime;
      public float pauseDuration;

      public static float GetRandomPauseDuration()
      {
        return (float) (1.0 + SamMath.Rand.NextDouble() * 1.0);
      }

      public static float GetRandomWanderDuration()
      {
        return (double) Time.time < 1.0 ? GooseConfig.settings.FirstWanderTimeSeconds : SamMath.RandomRange(GooseConfig.settings.MinWanderingTimeSeconds, GooseConfig.settings.MaxWanderingTimeSeconds);
      }

      public static float GetRandomWalkTime()
      {
        return SamMath.RandomRange(1f, 6f);
      }
    }

    private struct Task_NabMouse
    {
      public static readonly Vector2 StruggleRange = new Vector2(3f, 3f);
      public TheGoose.Task_NabMouse.Stage currentStage;
      public Vector2 dragToPoint;
      public float grabbedOriginalTime;
      public float chaseStartTime;
      public Vector2 originalVectorToMouse;
      public const float MouseGrabDistance = 15f;
      public const float MouseSuccTime = 0.06f;
      public const float MouseDropDistance = 30f;
      public const float MinRunTime = 2f;
      public const float MaxRunTime = 4f;
      public const float GiveUpTime = 9f;

      public enum Stage
      {
        SeekingMouse,
        DraggingMouseAway,
        Decelerating,
      }
    }

    private struct Task_CollectWindow
    {
      public TheGoose.MovableForm mainForm;
      public TheGoose.Task_CollectWindow.Stage stage;
      public float secsToWait;
      public float waitStartTime;
      public TheGoose.Task_CollectWindow.ScreenDirection screenDirection;
      public Vector2 windowOffsetToBeak;

      public static float GetWaitTime()
      {
        return SamMath.RandomRange(2f, 3.5f);
      }

      public enum Stage
      {
        WalkingOffscreen,
        WaitingToBringWindowBack,
        DraggingWindowBack,
      }

      public enum ScreenDirection
      {
        Left,
        Top,
        Right,
      }
    }

    private class MovableForm : Form
    {
      public MovableForm()
      {
        this.StartPosition = FormStartPosition.Manual;
        this.Width = 400;
        this.Height = 400;
        this.BackColor = Color.DimGray;
        this.Icon = (Icon) null;
        this.ShowIcon = false;
        this.SetWindowResizableThreadsafe(false);
      }

      public void SetWindowPositionThreadsafe(Point p)
      {
        if (this.InvokeRequired)
        {
          //this.BeginInvoke((Delegate) (() =>
          //{
          //  this.Location = p;
          //  this.TopMost = true;
          //}));
        }
        else
        {
          this.Location = p;
          this.TopMost = true;
        }
      }

      public void SetWindowResizableThreadsafe(bool canResize)
      {
        if (this.InvokeRequired)
        {
          //this.BeginInvoke((Delegate) (() =>
          //{
          //  this.FormBorderStyle = canResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
          //  this.MaximizeBox = this.MinimizeBox = canResize;
          //}));
        }
        else
        {
          this.FormBorderStyle = canResize ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle;
          this.MaximizeBox = this.MinimizeBox = canResize;
        }
      }
    }

    private class SimpleImageForm : TheGoose.MovableForm
    {
      private static readonly string memesRootFolder = Program.GetPathToFileInAssembly("Assets/Images/Memes/");
      private static string[] imageURLs = new string[5]
      {
        "https://preview.redd.it/dsfjv8aev0p31.png?width=960&crop=smart&auto=webp&s=1d58948acc5c6dd60df1092c1bd2a59a509069fd",
        "https://i.redd.it/4ojv59zvglp31.jpg",
        "https://i.redd.it/4bamd6lnso241.jpg",
        "https://i.redd.it/5i5et9p1vsp31.jpg",
        "https://i.redd.it/j2f1i9djx5p31.jpg"
      };
      private static Deck imageURLDeck = new Deck(TheGoose.SimpleImageForm.imageURLs.Length);
      private Image[] localImages;
      private Deck localImageDeck;

      public SimpleImageForm()
      {
        List<Image> imageList = new List<Image>();
        try
        {
          foreach (string file in Directory.GetFiles(TheGoose.SimpleImageForm.memesRootFolder))
          {
            Image image = Image.FromFile(file);
            if (image != null)
              imageList.Add(image);
          }
        }
        catch
        {
        }
        this.localImages = imageList.ToArray();
        this.localImageDeck = new Deck(this.localImages.Length);
        PictureBox pictureBox = new PictureBox();
        pictureBox.Dock = DockStyle.Fill;
        try
        {
          pictureBox.Image = this.localImages[this.localImageDeck.Next()];
        }
        catch
        {
          pictureBox.LoadAsync(TheGoose.SimpleImageForm.imageURLs[TheGoose.SimpleImageForm.imageURLDeck.Next()]);
        }
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        this.Controls.Add((Control) pictureBox);
      }
    }

    private class SimpleTextForm : TheGoose.MovableForm
    {
      private static string[] possiblePhrases = new string[6]
      {
        "am goose hjonk",
        "good work",
        "nsfdafdsaafsdjl\r\nasdas       sorry\r\nhard to type withh feet",
        "i cause problems on purpose",
        "\"peace was never an option\"\r\n   -the goose (me)",
        "\r\n\r\n  >o) \r\n    (_>"
      };
      private static Deck textIndices = new Deck(TheGoose.SimpleTextForm.possiblePhrases.Length);

      public SimpleTextForm()
      {
        this.Width = 200;
        this.Height = 150;
        this.Text = "Goose \"Not-epad\"";
        TextBox textBox = new TextBox();
        textBox.Multiline = true;
        textBox.AcceptsReturn = true;
        textBox.Text = TheGoose.SimpleTextForm.possiblePhrases[TheGoose.SimpleTextForm.textIndices.Next()];
        textBox.Location = new Point(0, 0);
        textBox.Width = this.ClientSize.Width;
        textBox.Height = this.ClientSize.Height - 5;
        textBox.Select(textBox.Text.Length, 0);
        textBox.Font = new Font(textBox.Font.FontFamily, 10f, FontStyle.Regular);
        this.Controls.Add((Control) textBox);
        string str = Environment.SystemDirectory + "\\notepad.exe";
        if (!File.Exists(str))
          return;
        try
        {
          this.Icon = Icon.ExtractAssociatedIcon(str);
          this.ShowIcon = true;
        }
        catch
        {
        }
      }

      private void ExitWindow(object sender, EventArgs args)
      {
        this.Close();
      }
    }

    private class SimpleDonateForm : TheGoose.MovableForm
    {
      private static string donationGraphicSrc = Program.GetPathToFileInAssembly("Assets/Images/OtherGfx/DonatePage.png");
      private float scale = 1.25f;
      private const string patreonLink = "https://www.patreon.com/bePatron?u=3541875";
      private const string paypalLink = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WUKYHY7SZ275Q&currency_code=USD&source=url";
      private const string twitterLink = "https://www.twitter.com/samnchiet";
      private const string discordLink = "https://discord.gg/PCJS6DH";

      public SimpleDonateForm()
      {
        PictureBox pictureBox = new PictureBox();
        this.ClientSize = new Size((int) (250.0 * (double) this.scale), (int) (300.0 * (double) this.scale));
        try
        {
          this.BackgroundImage = Image.FromFile(TheGoose.SimpleDonateForm.donationGraphicSrc);
        }
        catch
        {
          Label label = new Label();
          label.Text = "Can't find the donation image... are you messing with the game files?\nCheck out my Twitter at twitter.com/samnchiet I guess?";
          label.Location = new Point(0, 0);
          label.Width = this.ClientSize.Width;
          label.Height = this.ClientSize.Height;
          label.BackColor = Color.White;
          label.TextAlign = ContentAlignment.MiddleCenter;
          this.Controls.Add((Control) label);
        }
        this.BackgroundImageLayout = ImageLayout.Stretch;
        this.Controls.Add((Control) this.SetupButton(111, 407, 390, 475, new EventHandler(this.OpenPatreonLink), true));
        this.Controls.Add((Control) this.SetupButton(174, 500, 325, 545, new EventHandler(this.OpenPaypalLink), true));
        this.Controls.Add((Control) this.SetupButton(381, 302, 433, 360, new EventHandler(this.OpenDiscordLink), true));
        this.Controls.Add((Control) this.SetupButton(403, 247, 472, 312, new EventHandler(this.OpenTwitterLink), true));
      }

      private Button SetupButton(
        int point1X,
        int point1Y,
        int point2X,
        int point2Y,
        EventHandler handler,
        bool showHoverClick = true)
      {
        Button button = new Button();
        button.Location = new Point((int) ((double) point1X * (double) this.scale) / 2, (int) ((double) point1Y * (double) this.scale) / 2);
        button.Size = new Size((int) ((double) (point2X - point1X) * (double) this.scale) / 2, (int) ((double) (point2Y - point1Y) * (double) this.scale) / 2);
        button.Click += handler;
        button.Cursor = Cursors.Hand;
        button.BackColor = Color.Transparent;
        button.ForeColor = Color.Transparent;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.MouseOverBackColor = showHoverClick ? Color.FromArgb(40, Color.White) : Color.Transparent;
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, Color.White);
        button.FlatAppearance.BorderSize = 0;
        button.TabStop = false;
        return button;
      }

      private void OpenPatreonLink(object sender, EventArgs a)
      {
        Process.Start("https://www.patreon.com/bePatron?u=3541875");
      }

      private void OpenPaypalLink(object sender, EventArgs a)
      {
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=WUKYHY7SZ275Q&currency_code=USD&source=url");
      }

      private void OpenTwitterLink(object sender, EventArgs a)
      {
        Process.Start("https://www.twitter.com/samnchiet");
      }

      private void OpenDiscordLink(object sender, EventArgs a)
      {
        Process.Start("https://discord.gg/PCJS6DH");
      }
    }

    private struct Task_TrackMud
    {
      public const float DurationToRunAmok = 2f;
      public float nextDirChangeTime;
      public float timeToStopRunning;
      public TheGoose.Task_TrackMud.Stage stage;

      public static float GetDirChangeInterval()
      {
        return 100f;
      }

      public enum Stage
      {
        DecideToRun,
        RunningOffscreen,
        RunningWandering,
      }
    }

    private struct Rig
    {
      public const int UnderBodyRadius = 15;
      public const int UnderBodyLength = 7;
      public const int UnderBodyElevation = 9;
      public Vector2 underbodyCenter;
      public const int BodyRadius = 22;
      public const int BodyLength = 11;
      public const int BodyElevation = 14;
      public Vector2 bodyCenter;
      public const int NeccRadius = 13;
      public const int NeccHeight1 = 20;
      public const int NeccExtendForward1 = 3;
      public const int NeccHeight2 = 10;
      public const int NeccExtendForward2 = 16;
      public float neckLerpPercent;
      public Vector2 neckCenter;
      public Vector2 neckBase;
      public Vector2 neckHeadPoint;
      public const int HeadRadius1 = 15;
      public const int HeadLength1 = 3;
      public const int HeadRadius2 = 10;
      public const int HeadLength2 = 5;
      public Vector2 head1EndPoint;
      public Vector2 head2EndPoint;
      public const int EyeRadius = 2;
      public const int EyeElevation = 3;
      public const float IPD = 5f;
      public const float EyesForward = 5f;
    }
  }
}
