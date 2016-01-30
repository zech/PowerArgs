﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerArgs.Cli
{
    public static class Layout
    {
        public static void CenterVertically(Rectangular parent, Rectangular child)
        {
            var gap = parent.Height - child.Height;
            var y = gap / 2;
            child.Y = y;
        }

        public static void CenterHorizontally(Rectangular parent, Rectangular child)
        {
            var gap = parent.Width - child.Width;
            var x = gap / 2;
            child.X = x;
        }

        public static List<ConsoleControl> TraverseControlTree(ConsolePanel toTraverse)
        {
            List<ConsoleControl> ret = new List<ConsoleControl>();
            foreach (var control in toTraverse.Controls)
            {
                if (control is ConsolePanel)
                {
                    ret.AddRange(TraverseControlTree(control as ConsolePanel));
                }
                ret.Add(control);

            }
            return ret;
        }

        public static int StackHorizontally(int margin, IEnumerable<ConsoleControl> controls)
        {
            int left = 0;
            int width = 0;
            foreach (var control in controls)
            {
                control.X = left;
                width += control.Width;
                left += control.Width + margin;
            }
            return width;
        }



        public static int StackVertically(int margin, IEnumerable<ConsoleControl> controls)
        {
            int top = 0;
            int height = 0;
            foreach (var control in controls)
            {
                control.Y = top;
                height += control.Height;
                top += control.Height + margin;
            }
            return height;
        }

        public static int StackVertically(int margin, params ConsoleControl[] controls)
        {
            return StackVertically(margin, (IEnumerable<ConsoleControl>)controls);
        }

        public static int StackHorizontally(int margin, params ConsoleControl[] controls)
        {
            return StackHorizontally(margin, (IEnumerable<ConsoleControl>)controls);
        }

        public static T CenterVertically<T>(this T child, ConsoleControl parent = null) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;

            Action syncAction = () =>
            {
                var gap = parent.Height - child.Height;
                var y = gap / 2;
                child.Y = y;
            };
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();

            return child;
        }

        public static T CenterHorizontally<T>(this T child, ConsoleControl parent = null) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;

            Action syncAction = () =>
            {
                var gap = parent.Width - child.Width;
                var x = gap / 2;
                child.X = x;
            };
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();

            return child;
        }

        public static T Fill<T>(this T child, ConsoleControl parent = null, Thickness? padding = null) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;
            var effectivePadding = padding.HasValue ? padding.Value : new Thickness(0, 0, 0, 0);
            Action syncAction = () =>
            {
                var newBounds = new Rectangle(new Point(0, 0), parent.Size);
                newBounds.X += effectivePadding.Left;
                newBounds.Width -= effectivePadding.Left;
                newBounds.Width -= effectivePadding.Right;

                newBounds.Y += effectivePadding.Top;
                newBounds.Height -= effectivePadding.Top;
                newBounds.Height -= effectivePadding.Bottom;

                child.Bounds = newBounds;
            };
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }

        public static T FillHoriontally<T>(this T child, ConsoleControl parent = null, Thickness? padding = null) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;
            var effectivePadding = padding.HasValue ? padding.Value : new Thickness(0, 0, 0, 0);
            Action syncAction = () => 
            {
                child.Bounds = new Rectangle(effectivePadding.Left, child.Y, parent.Width - (effectivePadding.Right+effectivePadding.Left), child.Height);
            };
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }

        public static T FillVertically<T>(this T child, ConsoleControl parent = null) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;
            Action syncAction = () => { child.Bounds = new Rectangle(child.X, 0, child.Width, parent.Height); };
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }

        public static T DockToBottom<T>(this T child, ConsoleControl parent = null, int padding = 0) where T :ConsoleControl
        {
            parent = parent ?? child.Parent;
            Action syncAction = () =>
            {
                child.Y = parent.Height - child.Height - padding;
            };

            child.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }

        public static T DockToTop<T>(this T child, ConsoleControl parent = null, int padding = 0) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;
            Action syncAction = () =>
            {
                child.Y = padding;
            };

            child.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }

        public static T DockToRight<T>(this T child, ConsoleControl parent = null, int padding = 0) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;
            Action syncAction = () =>
            {
                child.X = parent.Width - child.Width - padding;
            };

            child.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }

        public static T DockToLeft<T>(this T child, ConsoleControl parent = null, int padding = 0) where T : ConsoleControl
        {
            parent = parent ?? child.Parent;
            Action syncAction = () =>
            {
                child.X = padding;
            };

            child.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            parent.Subscribe(nameof(ConsoleControl.Bounds), syncAction);
            syncAction();
            return child;
        }
    }
}

