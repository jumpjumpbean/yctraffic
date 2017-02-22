﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace WafTraffic.Reporting.Presentation.Controls
{
    public class ItemsElement : Section
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsElement), new UIPropertyMetadata(null, ItemsSourceChangedHandler));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ItemsElement), new UIPropertyMetadata(null));


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }


        private void UpdateContent()
        {
            if (ItemsSource != null)
            {
                if (ItemTemplate == null)
                {
                    throw new InvalidOperationException("When ItemsSource is used then the ItemTemplate must not be null.");
                }

                List<Block> blocks = new List<Block>();
                foreach (object item in ItemsSource)
                {
                    Block block = null;
                    ContentElement contentElement = ItemTemplate.LoadContent() as ContentElement;
                    if (contentElement != null)
                    {
                        block = contentElement.Content as Block;
                    }

                    if (block == null)
                    {
                        throw new InvalidOperationException("The ItemTemplate must define: DataTemplate > ContentElement > Block element.");
                    }
                    block.DataContext = item;
                    blocks.Add(block);
                }

                Blocks.AddRange(blocks);
            }
        }

        private static void ItemsSourceChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ItemsElement itemsElement = (ItemsElement)d;
            itemsElement.UpdateContent();
        }
    }
}
