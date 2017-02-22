import codecs,sys
import os
import tempfile

ls = os.linesep  
gPathPresentation = os.getcwd() + os.path.sep + "WafTraffic.Presentation"
gPathApplication = os.getcwd() + os.path.sep + "WafTraffic.Applications"
gPathDomain = os.getcwd() + os.path.sep + "WafTraffic.Domain"
gPathPresViews = gPathPresentation + os.path.sep + "Views"
gPathAppViews = gPathApplication + os.path.sep + "Views"
gPathAppViewModels = gPathApplication + os.path.sep + "ViewModels"
gPathAppControllers = gPathApplication + os.path.sep + "Controllers"
gPathAppServices = gPathApplication + os.path.sep + "Services"

def pathPrecheck():    
    return os.path.exists(gPathPresentation) \
           and os.path.exists(gPathApplication) \
           and os.path.exists(gPathDomain)

def fileCreatedCheck(moduleName):
    xamlFile = gPathPresViews + os.path.sep + "SquadronLogbook" + os.path.sep + moduleName + "View.xaml"
    return os.path.exists(xamlFile)

def createPresUpdateViews(prefix, moduleName):
    xamlContent = '''<UserControl x:Class="WafTraffic.Presentation.Views.Lb{module}UpdateView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:themes="clr-namespace:WPF.Themes;assembly=WPF.Themes"
             xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit"
             d:DesignHeight="1247"
             d:DesignWidth="901"
             FontFamily="Microsoft YaHei"
             themes:ThemeManager.Theme="WhistlerBlue"
             mc:Ignorable="d">
    <Grid Height="821" Width="898">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="65" />
            <ColumnDefinition Width="193" />
            <ColumnDefinition Width="350" />
            <ColumnDefinition Width="120" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="25" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="60" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <TextBox Grid.Row="2"
                 Grid.Column="2"
                 Margin="8,14,68,14"
                 VerticalAlignment="Center"
                 IsReadOnly="{{Binding Path=IsBrowse}}"
                 Text="{{Binding {parm}Entity.Name}}" Height="33" Width="275" />
        <TextBox Grid.Row="3"
                 Grid.Column="2"
                 Margin="8,14,68,14"
                 VerticalAlignment="Center"
                 IsReadOnly="{{Binding Path=IsBrowse}}"
                 Text="{{Binding {parm}Entity.PoliceNo}}" Height="33" Width="275" />
        <TextBox Grid.Row="4"
                 Grid.Column="2"
                 Margin="8,16,68,12"
                 VerticalAlignment="Center"
                 IsReadOnly="{{Binding Path=IsBrowse}}"
                 Text="{{Binding {parm}Entity.Telephone}}" Height="33" Width="275" />
        <TextBox Grid.Row="5"
                 Grid.Column="2"
                 Margin="8,16,68,12"
                 VerticalAlignment="Center"
                 IsReadOnly="{{Binding Path=IsBrowse}}"
                 Text="{{Binding {parm}Entity.Address}}" Height="33" Width="275" />
        <TextBox Grid.Row="6"
                 Grid.Column="2"
                 Margin="8,14,68,14"
                 VerticalAlignment="Center"
                 IsReadOnly="{{Binding Path=IsBrowse}}"
                 Text="{{Binding {parm}Entity.Degree}}" Height="33" Width="275" />
        <TextBox Grid.Row="7"
                 Grid.Column="2"
                 Margin="8,15,68,11"
                 VerticalAlignment="Center"
                 IsReadOnly="{{Binding Path=IsBrowse}}"
                 Text="{{Binding {parm}Entity.IdNo}}" Height="33" Width="275" />

        <Button x:Name="btnSave"
                Grid.Row="8"
                Grid.Column="2"
                Width="90"
                Margin="40,10,220,0"
                Command="{{Binding Path=SaveCommand}}"
                Height="50">
            <Button.Content>
                <StackPanel Orientation="Horizontal">
                    <Image Source ="/Resources/Images/icon_button_save.png" 
                           Width="32" Height="32" />
                    <TextBlock Text="保 存" Foreground="DarkSlateBlue" FontSize="15" HorizontalAlignment="Center" VerticalAlignment="Center" />
                </StackPanel>
            </Button.Content>
        </Button>

        <Button x:Name="btnCancel"
                Grid.Row="8"
                Grid.Column="2"
                Width="90"
                Margin="136,10,124,0"
                Command="{{Binding Path=CancelCommand}}"
                Height="50" >
            <Button.Content>
                <StackPanel Orientation="Horizontal">
                    <Image Source ="/Resources/Images/icon_button_cancel.png" 
                           Width="32" Height="32" />
                    <TextBlock Text="取 消" Foreground="DarkSlateBlue" FontSize="15" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                </StackPanel>
            </Button.Content>
        </Button>
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,16,8,11" Text="身份证号 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="7" />
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,14,8,13" Text="文化程度 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="6" />
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,15,8,12" Text="现住址 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="5" />
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,22,8,5" Text="电话 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="4" />
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,19,8,8" Text="警号 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="3" />
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,14,8,13" Text="姓名 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="2" />
        <TextBlock FontSize="15" FontStretch="Normal" FontStyle="Normal" FontWeight="Normal" Height="33" HorizontalAlignment="Center" Margin="65,18,8,9" Text="台账录入时间 :" TextAlignment="Right" VerticalAlignment="Center" Width="120" Grid.Column="1" Grid.Row="1" />
        <DatePicker BorderThickness="1" FontSize="15" Height="34" HorizontalAlignment="Left" Margin="8,17,0,0" x:Name="dpRecordTime" SelectedDate="{{Binding {parm}Entity.RecordTime}}" VerticalAlignment="Top" Width="275" Grid.Column="2" Grid.Row="1" />
        <Button x:Name="btnBack" Command="{{Binding Path=CancelCommand}}" Height="50" Margin="90,10,170,0" Width="90" Grid.Column="2" Grid.Row="8">
            <StackPanel Orientation="Horizontal">
                <Image Height="32" Source="pack://application:,,,/Resources/Images/icon_button_cancel.png" Width="32" />
                <TextBlock FontSize="15" Foreground="DarkSlateBlue" HorizontalAlignment="Center" Text="返 回" VerticalAlignment="Center" />
            </StackPanel>
        </Button>
        <TextBlock Foreground="Red" Margin="289,23,41,21" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Column="2" Grid.Row="1" />
        <TextBlock Foreground="Red" Margin="289,22,41,22" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Column="2" Grid.Row="2" />
        <TextBlock Foreground="Red" Margin="289,22,41,22" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Column="2" Grid.Row="3" />
        <TextBlock Foreground="Red" Margin="289,25,41,19" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Column="2" Grid.Row="4" />
        <TextBlock Foreground="Red" Margin="289,23,41,21" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Column="2" Grid.Row="5" />
        <TextBlock Foreground="Red" Margin="289,21,41,23" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Row="6" Grid.Column="2" />
        <TextBlock Foreground="Red" Margin="289,22,41,22" Text="*" TextAlignment="Right" VerticalAlignment="Center" Grid.Row="7" Grid.Column="2" />
    </Grid>
</UserControl>
    '''.format(module = prefix + moduleName, parm = moduleName)
    # create xaml file under .\WafTraffic\WafTraffic.Presentation\Views
    xamlFile = gPathPresViews + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix + moduleName + "UpdateView.xaml"
    fobj = open(xamlFile, 'w')
    fobj.write(xamlContent)
    fobj.close()  
    print ('{} file created!'.format("Lb" + prefix + moduleName + "UpateView.xaml"))

    xamlCsContent = '''using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using System.Windows;

namespace WafTraffic.Presentation.Views
{{
    /// <summary>
    /// Lb{module}View.xaml的交互逻辑
    /// </summary>
    [Export(typeof(ILb{module}UpdateView))]
    public partial class Lb{module}UpdateView  : UserControl, ILb{module}UpdateView
    {{
        private readonly Lazy<Lb{module}UpdateViewModel> viewModel;

        public Lb{module}UpdateView()
        {{
            InitializeComponent();
            viewModel = new Lazy<Lb{module}UpdateViewModel>(() => ViewHelper.GetViewModel<Lb{module}UpdateViewModel>(this));

            Loaded += LoadedHandler;
        }}

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {{
            this.dpRecordTime.IsEnabled = viewModel.Value.IsNewOrModify;
            this.btnBack.Visibility = viewModel.Value.BrowseVisibility;
            this.btnSave.Visibility = viewModel.Value.NewOrModifyVisibility;
            this.btnCancel.Visibility = viewModel.Value.NewOrModifyVisibility;
        }}
    }}
}}
    '''.format(module = prefix + moduleName)
    # create xaml.cs file under .\WafTraffic\WafTraffic.Presentation\Views
    xamlCsFile = gPathPresViews + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix + moduleName + "UpdateView.xaml.cs"
    fobj2 = open(xamlCsFile, 'w')
    fobj2.write(xamlCsContent)
    fobj2.close()  
    print ('{} file created!'.format("Lb" + prefix + moduleName + "UpdateView.xaml.cs"))

def createPresQueryViews(prefix, moduleName):
    xamlContent = '''<UserControl x:Class="WafTraffic.Presentation.Views.Lb{module}QueryView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:my="clr-namespace:CustomControlLibrary;assembly=CustomControlLibrary"
             xmlns:themes="clr-namespace:WPF.Themes;assembly=WPF.Themes"
             xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit"
             d:DesignHeight="412"
             d:DesignWidth="969"
             FontFamily="Microsoft YaHei"
             FontSize="15"
             themes:ThemeManager.Theme="WhistlerBlue"
             mc:Ignorable="d">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="80" />
            <RowDefinition />
        </Grid.RowDefinitions>
        <Grid>
            <StackPanel Orientation="Horizontal">
                <TextBlock Width="70"
                           HorizontalAlignment="Right"
                           VerticalAlignment="Center"
                           Text="开始时间:"
                           TextAlignment="Right" />
                <DatePicker x:Name="dpStartDate"
                            Width="150"
                            Margin="10 6 10 6"
                            VerticalAlignment="Center"
                            VerticalContentAlignment="Center"
                            FirstDayOfWeek="Monday"
                            IsTodayHighlighted="True"
                            SelectedDate="{{Binding Path=SelectedStartDate}}"
                            SelectedDateFormat="Long" />
                <TextBlock Width="70"
                           HorizontalAlignment="Right"
                           VerticalAlignment="Center"
                           Text="结束时间:"
                           TextAlignment="Right" />
                <DatePicker x:Name="dpEndDate"
                            Width="150"
                            Margin="10 6 10 6"
                            VerticalAlignment="Center"
                            VerticalContentAlignment="Center"
                            FirstDayOfWeek="Monday"
                            IsTodayHighlighted="True"
                            SelectedDate="{{Binding Path=SelectedEndDate}}"
                            SelectedDateFormat="Long" />
                <TextBlock Width="40"
                           HorizontalAlignment="Right"
                           VerticalAlignment="Center"
                           Text="科室:"
                           TextAlignment="Right" />
                <ComboBox x:Name="cmbDepartment"
                          Width="120"
                          Margin="10 6 10 6"
                          HorizontalAlignment="Left"
                          VerticalAlignment="Center"
                          DisplayMemberPath="FullName"
                          IsEnabled="{{Binding Path=IsSelectDepartmentEnabled}}"
                          ItemsSource="{{Binding DepartmentList}}"
                          SelectedValue="{{Binding Path=SelectedDepartment}}"
                          SelectedValuePath="Id" />
                <Button Width="90"
                        Height="40"
                        Margin="10 6 10 6"
                        VerticalAlignment="Center"
                        Command="{{Binding Path=QueryCommand}}" >
                    <Button.Content>
                        <StackPanel Orientation="Horizontal">
                            <Image Source ="/Resources/Images/icon_button_query.png" 
                           Width="25" Height="25" />
                            <TextBlock Text="查 询" Foreground="DarkSlateBlue" FontSize="15" HorizontalAlignment="Center" VerticalAlignment="Center" />
                        </StackPanel>
                    </Button.Content>
                </Button>
                <Button Width="90"
                        Height="40"
                        Margin="10 6 10 6"
                        VerticalAlignment="Center"
                        Visibility="{{Binding Path=AddPermissionVisibility}}"
                        Command="{{Binding Path=NewCommand}}" >
                    <Button.Content>
                        <StackPanel Orientation="Horizontal">
                            <Image Source ="/Resources/Images/icon_button_add.png" 
                           Width="25" Height="25" />
                            <TextBlock Text="新 增" Foreground="DarkSlateBlue" FontSize="15" HorizontalAlignment="Center" VerticalAlignment="Center" />
                        </StackPanel>
                    </Button.Content>
                </Button>
            </StackPanel>
        </Grid>
        <ScrollViewer Grid.Row="2"
                      HorizontalScrollBarVisibility="Auto"
                      VerticalScrollBarVisibility="Auto">
            <my:PagingDataGrid Name="grid{parm}List"
                               AlternationCount="2"
                               AutoGenerateColumns="False"
                               Background="Transparent"
                               BorderBrush="LightBlue"
                               BorderThickness="1"
                               HorizontalGridLinesBrush="AliceBlue"
                               PageSize="25"
                               PageSizeList="10,20,25,30,40"
                               PagingChanged="grid{parm}List_PagingChanged"
                               IsReadOnly="True"
                               RowBackground="{{DynamicResource {{x:Static SystemColors.GradientInactiveCaptionBrushKey}}}}" ColumnHeaderHeight="40"
                               SelectedItem="{{Binding Selected{parm}}}"
                               SelectionMode="Single"
                               VerticalGridLinesBrush="AliceBlue" >
                <DataGrid.Columns>
                    <DataGridTextColumn Width="150" Binding="{{Binding Name}}" Header="姓名" />
                    <DataGridTextColumn Width="150" Binding="{{Binding RecordTime, Converter={{StaticResource DateConverter}}}}" Header="创建时间" />
                    <DataGridTextColumn Width="150" Binding="{{Binding PoliceNo}}" Header="警号" />
                    <DataGridTextColumn Width="150"
                                        Binding="{{Binding Telephone}}"
                                        Header="电话" />
                    <DataGridTemplateColumn Header="查看" Width="80" >
                        <DataGridTemplateColumn.CellTemplate>
                            <DataTemplate>
                                <Grid>
                                    <Image Source="/Resources/Images/Browse.png" Grid.Row="0" Stretch="Fill" Width="24" Height="24"/>
                                    <Button Content="查看" Margin="3" Grid.Row="0" Width="24" Height="24" 
                                        Command="{{Binding DataContext.BrowseCommand, RelativeSource={{RelativeSource Mode=FindAncestor, AncestorType=DataGrid}}}}"                                            
                                        CommandParameter="{{Binding RelativeSource={{RelativeSource Mode=FindAncestor, AncestorType=DataGrid}}, Path=SelectedItem}}">
                                        <Button.OpacityMask>
                                            <LinearGradientBrush StartPoint="0,70" EndPoint="3,0">
                                                <GradientStop Offset="0" Color="Black"/>
                                                <GradientStop Offset="1" Color="Transparent"/>
                                            </LinearGradientBrush>
                                        </Button.OpacityMask>
                                    </Button>
                                </Grid>
                            </DataTemplate>
                        </DataGridTemplateColumn.CellTemplate>
                    </DataGridTemplateColumn>

                    <DataGridTemplateColumn Header="编辑" Width="80" >
                        <DataGridTemplateColumn.CellTemplate>
                            <DataTemplate>
                                <Grid>
                                    <Image Source="/Resources/Images/Modify.png" Grid.Row="0" Stretch="Fill" Width="24" Height="24"/>
                                    <Button Content="修改" Margin="3" Grid.Row="0" Width="24" Height="24" 
                                        Command="{{Binding DataContext.ModifyCommand, RelativeSource={{RelativeSource Mode=FindAncestor, AncestorType=DataGrid}}}}" 
                                        CommandParameter="{{Binding RelativeSource={{RelativeSource Mode=FindAncestor, AncestorType=DataGrid}}, Path=SelectedItem}}">
                                        <Button.OpacityMask>
                                            <LinearGradientBrush StartPoint="0,70" EndPoint="3,0">
                                                <GradientStop Offset="0" Color="Black"/>
                                                <GradientStop Offset="1" Color="Transparent"/>
                                            </LinearGradientBrush>
                                        </Button.OpacityMask>
                                    </Button>
                                </Grid>
                            </DataTemplate>
                        </DataGridTemplateColumn.CellTemplate>
                    </DataGridTemplateColumn>
                    <DataGridTemplateColumn Header="删除" Width="80" >
                        <DataGridTemplateColumn.CellTemplate>
                            <DataTemplate>
                                <Grid>
                                    <Image Source="/Resources/Images/DeleteGrid.png" Grid.Row="0" Stretch="Fill" Width="24" Height="24"/>
                                    <Button Content="删除" Margin="3" Grid.Row="0" Width="24" Height="24" 
                                        Command="{{Binding DataContext.DeleteCommand, RelativeSource={{RelativeSource Mode=FindAncestor, AncestorType=DataGrid}}}}"                                            
                                        CommandParameter="{{Binding RelativeSource={{RelativeSource Mode=FindAncestor, AncestorType=DataGrid}}, Path=SelectedItem}}">
                                        <Button.OpacityMask>
                                            <LinearGradientBrush StartPoint="0,70" EndPoint="3,0">
                                                <GradientStop Offset="0" Color="Black"/>
                                                <GradientStop Offset="1" Color="Transparent"/>
                                            </LinearGradientBrush>
                                        </Button.OpacityMask>
                                    </Button>
                                </Grid>
                            </DataTemplate>
                        </DataGridTemplateColumn.CellTemplate>
                    </DataGridTemplateColumn>
                </DataGrid.Columns>

            </my:PagingDataGrid>
        </ScrollViewer>

    </Grid>
</UserControl>
    '''.format(module = prefix + moduleName, parm = moduleName)
    # create xaml file under .\WafTraffic\WafTraffic.Presentation\Views
    xamlFile = gPathPresViews + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix + moduleName + "QueryView.xaml"
    fobj = open(xamlFile, 'w')
    fobj.write(xamlContent)
    fobj.close()  
    print ('{} file created!'.format("Lb" + prefix + moduleName + "QueryView.xaml"))

    xamlCsContent = '''using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Waf.Applications;
using System.Windows;
using WafTraffic.Domain;
using System.Windows.Controls;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using DotNet.Business;
using System.Data;

namespace WafTraffic.Presentation.Views
{{
    /// <summary>
    /// Lb{module}QueryView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ILb{module}QueryView))]
    public partial class Lb{module}QueryView  : UserControl, ILb{module}QueryView
    {{
        private readonly Lazy<Lb{module}QueryViewModel> viewModel;

        public Lb{module}QueryView()
        {{
            InitializeComponent();
            viewModel = new Lazy<Lb{module}QueryViewModel>(() => ViewHelper.GetViewModel<Lb{module}QueryViewModel>(this));

            Loaded += FirstTimeLoadedHandler;
        }}

        private void FirstTimeLoadedHandler(object sender, RoutedEventArgs e)
        {{
            this.dpStartDate.Text = DateTime.Today.ToLongDateString();
            this.dpEndDate.Text = DateTime.Today.ToLongDateString();

            this.grid{parm}List.Columns[4].Visibility = viewModel.Value.BrowsePermissionVisibility;
            this.grid{parm}List.Columns[5].Visibility = viewModel.Value.ModifyPermissionVisibility;
            this.grid{parm}List.Columns[6].Visibility = viewModel.Value.DeletePermissionVisibility;
        }}

        private void grid{parm}List_PagingChanged(object sender, CustomControlLibrary.PagingChangedEventArgs args)
        {{
            int startIndex = (args.PageIndex - 1) * args.PageSize;
            if (startIndex > grid{parm}List.Total)
            {{
                startIndex = (grid{parm}List.Total / args.PageSize) * args.PageSize;
                grid{parm}List.PageIndex = (grid{parm}List.Total % args.PageSize) == 0 ? (grid{parm}List.Total / args.PageSize) : (grid{parm}List.Total / args.PageSize) + 1;
                args.PageIndex = grid{parm}List.PageIndex;
            }}

            IEnumerable<Zdtz{module}> gridTables = viewModel.Value.Lb{parm}s;

            grid{parm}List.Total = gridTables.Count();
            grid{parm}List.ItemsSource = gridTables.Skip((args.PageIndex - 1) * args.PageSize).Take(args.PageSize);
        }}

        public void PagingReload()
        {{
            grid{parm}List.RaisePageChanged();
        }}
    }}
}}
    '''.format(module = prefix + moduleName, parm = moduleName)
    # create xaml.cs file under .\WafTraffic\WafTraffic.Presentation\Views
    xamlCsFile = gPathPresViews + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix + moduleName + "QueryView.xaml.cs"
    fobj2 = open(xamlCsFile, 'w')
    fobj2.write(xamlCsContent)
    fobj2.close()  
    print ('{} file created!'.format("Lb" + prefix + moduleName + "QueryView.xaml.cs"))



def updatePresCsproj(prefix, moduleName):
    file = gPathPresentation + os.path.sep + "WafTraffic.Presentation.csproj"
    fobj = codecs.open(file, 'rU', 'utf-8')
    
    content = fobj.read()

    fobj.close()

    content = content.replace('</Compile>\r\n    <Page', \
                                 '</Compile>\r\n    <Compile Include="Views\\SquadronLogbook\\Lb{module}UpdateView.xaml.cs">\r\n\
      <DependentUpon>Lb{module}UpdateView.xaml</DependentUpon>\r\n    </Compile>\r\n    <Compile Include="Views\\SquadronLogbook\\Lb{module}QueryView.xaml.cs">\r\n\
      <DependentUpon>Lb{module}QueryView.xaml</DependentUpon>\r\n    </Compile>\r\n    <Page'.format(module = prefix + moduleName))
    content = content.replace('</Page>\r\n  </ItemGroup>', \
                              '</Page>\r\n    <Page Include="Views\\SquadronLogbook\\Lb{module}UpdateView.xaml">\r\n\
      <Generator>MSBuild:Compile</Generator>\r\n      <SubType>Designer</SubType>\r\n\
    </Page>\r\n    <Page Include="Views\\SquadronLogbook\\Lb{module}QueryView.xaml">\r\n\
      <Generator>MSBuild:Compile</Generator>\r\n      <SubType>Designer</SubType>\r\n\
    </Page>\r\n  </ItemGroup>'.format(module = prefix + moduleName))

    fobj2 = codecs.open(file, 'w', 'utf-8')
    fobj2.write(content)
    fobj2.close()  
    print ('WafTraffic.Presentation.csproj updated!')
    return

def createAppViews(prefix, moduleName):
    content = '''using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace WafTraffic.Applications.Views
{{
    public interface ILb{module}UpdateView : IView
    {{
    }}
}}
    '''.format(module = prefix + moduleName)
    file = gPathAppViews + os.path.sep + "SquadronLogbook" + os.path.sep + "ILb" + prefix +  moduleName + "UpdateView.cs"
    fobj = open(file, 'w')
    fobj.write(content)
    fobj.close()  
    print ('{} file created!'.format("ILb" + prefix + moduleName + "UpdateView.cs"))

    content2 = '''using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace WafTraffic.Applications.Views
{{
    public interface ILb{module}QueryView : IView
    {{
        void PagingReload();
    }}
}}
    '''.format(module = prefix + moduleName)
    file2 = gPathAppViews + os.path.sep + "SquadronLogbook" + os.path.sep + "ILb" + prefix +  moduleName + "QueryView.cs"
    fobj2 = open(file2, 'w')
    fobj2.write(content2)
    fobj2.close()  
    print ('{} file created!'.format("ILb" + prefix + moduleName + "QueryView.cs"))

def createAppViewModels(prefix, moduleName):
    content = '''using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using System.Windows;
using WafTraffic.Applications.Common;

namespace WafTraffic.Applications.ViewModels
{{
    [Export]
    public class Lb{module}QueryViewModel : LbBaseQueryViewModel<ILb{module}QueryView>
    {{
        #region Data

        private Zdtz{module} mSelected{parm};
        private IEnumerable<Zdtz{module}> mLb{parm}s;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public Lb{module}QueryViewModel(ILb{module}QueryView view, IEntityService entityservice)
            : base(view, entityservice)
        {{
            if (entityservice.EnumLb{parm}s != null)
            {{
                this.mLb{parm}s = entityservice.EnumLb{parm}s;
            }}
            else
            {{
                this.mLb{parm}s = new List<Zdtz{module}>(); //以防没有数据时出现异常
            }}

        }}

        #endregion

        #region Properties

        public IEnumerable<Zdtz{module}> Lb{parm}s
        {{
            get
            {{
                return mLb{parm}s;
            }}
            set
            {{
                mLb{parm}s = value;
                RaisePropertyChanged("Lb{parm}s");
            }}
        }}

        public Zdtz{module} Selected{parm}
        {{
            get {{ return mSelected{parm}; }}
            set
            {{
                if (mSelected{parm} != value)
                {{
                    mSelected{parm} = value;
                    RaisePropertyChanged("Selected{parm}");
                }}
            }}
        }}

        #endregion

        #region Members

        public override void GridRefresh()
        {{
             ViewCore.PagingReload();
        }}

        #endregion
    }}
}}
    '''.format(module = prefix + moduleName, parm = moduleName)
    file = gPathAppViewModels + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix +  moduleName + "QueryViewModel.cs"
    fobj = open(file, 'w')
    fobj.write(content)
    fobj.close()  
    print ('{} file created!'.format("Lb" + prefix +  moduleName + "QueryViewModel.cs"))    

    content2 = '''using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Waf.Applications;
using System.Windows.Input;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System.ComponentModel.Composition;
using WafTraffic.Applications.Services;
using DotNet.Business;
using System.Data;
using System.Windows;

namespace WafTraffic.Applications.ViewModels
{{
    [Export]
    public class Lb{module}UpdateViewModel : LbBaseUpdateViewModel<ILb{module}UpdateView>
    {{
        #region Data

        private Zdtz{module} m{parm}Entity;

        #endregion

        #region Constructors

        [ImportingConstructor]
        public Lb{module}UpdateViewModel(ILb{module}UpdateView view)
            : base(view)
        {{
            m{parm}Entity = new Zdtz{module}();
        }}

        #endregion

        #region Properties

        public Zdtz{module} {parm}Entity
        {{
            get {{ return m{parm}Entity; }}
            set
            {{
                if (m{parm}Entity != value)
                {{
                    m{parm}Entity = value;
                    RaisePropertyChanged("{parm}Entity");
                }}
            }}
        }}

        #endregion
    }}
}}
    '''.format(module = prefix + moduleName, parm = moduleName)
    file2 = gPathAppViewModels + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix +  moduleName + "UpdateViewModel.cs"
    fobj2 = open(file2, 'w')
    fobj2.write(content2)
    fobj2.close()  
    print ('{} file created!'.format("Lb" + prefix +  moduleName + "UpdateViewModel.cs")) 

def createController(prefix, moduleName):
    content = '''using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Applications.Services;
using System.Waf.Foundation;
using WafTraffic.Applications.Properties;
using WafTraffic.Applications.Services;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;
using WafTraffic.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using DotNet.Business;
using System.Windows.Forms;
using WafTraffic.Applications.Common;
using DotNet.Utilities;

namespace WafTraffic.Applications.Controllers
{{
    [Export]
    internal class Lb{module}Controller : Controller
    {{
        private readonly CompositionContainer mContainer;
        private readonly IShellService mShellService;
        private readonly System.Waf.Applications.Services.IMessageService mMessageService;
        private readonly IEntityService mEntityService;

        private LogbookMainViewModel mLogbookMainViewModel;
        private Lb{module}UpdateViewModel m{parm}ViewModel;
        private Lb{module}QueryViewModel m{parm}QueryViewModel;

        private readonly DelegateCommand mNewCommand;
        private readonly DelegateCommand mModifyCommand;
        private readonly DelegateCommand mDeleteCommand;
        private readonly DelegateCommand mBrowseCommand;
        private readonly DelegateCommand mSaveCommand;
        private readonly DelegateCommand mQueryCommand;
        private readonly DelegateCommand mCancelCommand;

        [ImportingConstructor]
        public Lb{module}Controller(CompositionContainer container, 
            System.Waf.Applications.Services.IMessageService messageService, 
            IShellService shellService, IEntityService entityService)
        {{
            try
            {{
                this.mContainer = container;
                this.mMessageService = messageService;
                this.mShellService = shellService;
                this.mEntityService = entityService;

                mLogbookMainViewModel = container.GetExportedValue<LogbookMainViewModel>();
                m{parm}ViewModel = container.GetExportedValue<Lb{module}UpdateViewModel>();
                m{parm}QueryViewModel = container.GetExportedValue<Lb{module}QueryViewModel>();

                this.mNewCommand = new DelegateCommand(() => NewOper(), null);
                this.mModifyCommand = new DelegateCommand(() => ModifyOper(), null);
                this.mDeleteCommand = new DelegateCommand(() => DeleteOper(), null);
                this.mSaveCommand = new DelegateCommand(() => Save(), null);
                this.mQueryCommand = new DelegateCommand(() => QueryOper(), null);
                this.mCancelCommand = new DelegateCommand(() => CancelOper(), null);
                this.mBrowseCommand = new DelegateCommand(() => BrowseOper(), null);
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}
        }}

        public void Initialize()
        {{
            try
            {{
                m{parm}ViewModel.SaveCommand = this.mSaveCommand;
                m{parm}ViewModel.CancelCommand = this.mCancelCommand;

                m{parm}QueryViewModel.NewCommand = this.mNewCommand;
                m{parm}QueryViewModel.ModifyCommand = this.mModifyCommand;
                m{parm}QueryViewModel.DeleteCommand = this.mDeleteCommand;
                m{parm}QueryViewModel.BrowseCommand = this.mBrowseCommand;
                m{parm}QueryViewModel.QueryCommand = this.mQueryCommand;
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}
        }}

        public bool NewOper()
        {{
            bool newer = true;

            try
            {{
                m{parm}ViewModel.IsBrowse = false;
                m{parm}ViewModel.{parm}Entity = new Zdtz{module}();
                mLogbookMainViewModel.ContentView = m{parm}ViewModel.View;
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}

            return newer;
        }}

        public bool ModifyOper()
        {{
            bool newer = true;

            try
            {{
                m{parm}ViewModel.IsBrowse = false;
                m{parm}ViewModel.{parm}Entity = m{parm}QueryViewModel.Selected{parm};
                mLogbookMainViewModel.ContentView = m{parm}ViewModel.View;
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}

            return newer;
        }}

        public bool BrowseOper()
        {{
            bool newer = true;

            try
            {{
                m{parm}ViewModel.IsBrowse = true;
                m{parm}ViewModel.{parm}Entity = m{parm}QueryViewModel.Selected{parm};
                mLogbookMainViewModel.ContentView = m{parm}ViewModel.View;
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}

            return newer;
        }}

        public bool DeleteOper()
        {{            
            bool deleter = false;

            try
            {{
                DialogResult result = MessageBox.Show("确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {{
                    m{parm}QueryViewModel.Selected{parm}.IsDeleted = YcConstants.INT_DB_DATA_DELETED;
                    m{parm}QueryViewModel.Selected{parm}.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    m{parm}QueryViewModel.Selected{parm}.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges();
                    m{parm}QueryViewModel.GridRefresh();
                }}
                deleter = true;
            }}
            catch (Exception ex)
            {{
                MessageBox.Show("发生错误，删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }}

            return deleter;
        }}

        public bool CancelOper()
        {{
            bool newer = true;

            try
            {{
                if (m{parm}QueryViewModel.Selected{parm} != null && m{parm}QueryViewModel.Selected{parm}.Id != 0)
                {{
                    mEntityService.Entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, m{parm}QueryViewModel.Selected{parm});
                }}
                mLogbookMainViewModel.ContentView = m{parm}QueryViewModel.View;
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}

            return newer;
        }}

        public bool QueryOper()
        {{
            bool newer = true;
            int deptId;
            try
            {{
                if (m{parm}QueryViewModel.IsSelectDepartmentEnabled)
                {{
                    deptId = Convert.ToInt32(m{parm}QueryViewModel.SelectedDepartment);
                }}
                else
                {{
                    deptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);
                }}

                DateTime startDate = m{parm}QueryViewModel.SelectedStartDate;
                DateTime endDate = m{parm}QueryViewModel.SelectedEndDate;

                m{parm}QueryViewModel.Lb{parm}s = mEntityService.Query{parm}s(deptId, startDate, endDate);
                if (m{parm}QueryViewModel.Lb{parm}s == null || m{parm}QueryViewModel.Lb{parm}s.Count() == 0)
                {{
                    MessageBox.Show("无符合条件数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }}
                m{parm}QueryViewModel.GridRefresh();
            }}
            catch (System.Exception ex)
            {{
                CurrentLoginService.Instance.LogException(ex);
            }}

            return newer;
        }}

        public bool Save()
        {{
            bool saved = false;

            try
            {{
                if (!ValueCheck())
                {{
                    return saved;
                }}
                if (m{parm}ViewModel.{parm}Entity.Id > 0)
                {{
                    m{parm}ViewModel.{parm}Entity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    m{parm}ViewModel.{parm}Entity.UpdateTime = System.DateTime.Now;
                    mEntityService.Entities.SaveChanges(); //update
                }}
                else
                {{
                    m{parm}ViewModel.{parm}Entity.ConfigId = mLogbookMainViewModel.SelectedLogbook.Id;
                    m{parm}ViewModel.{parm}Entity.OwnDepartmentId = AuthService.Instance.GetSquadronLogbookOwnDept();
                    m{parm}ViewModel.{parm}Entity.CreateId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    m{parm}ViewModel.{parm}Entity.CreateTime = System.DateTime.Now;

                    m{parm}ViewModel.{parm}Entity.UpdaterId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.Id);
                    m{parm}ViewModel.{parm}Entity.UpdateTime = System.DateTime.Now;
                    m{parm}ViewModel.{parm}Entity.IsDeleted = YcConstants.INT_DB_DATA_AVAILABLE;

                    mEntityService.Lb{parm}s.Add(m{parm}ViewModel.{parm}Entity);

                    mEntityService.Entities.SaveChanges(); //insert
                }}
                saved = true;

                MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //mMessageService.ShowMessage(mShellService.ShellView, string.Format(CultureInfo.CurrentCulture, "保存成功."));
                //返回列表页
                mLogbookMainViewModel.ContentView = m{parm}QueryViewModel.View;
            }}
            catch (ValidationException ex)
            {{
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }}
            catch (UpdateException ex)
            {{
                MessageBox.Show("发生错误，保存失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }}

            return saved;
        }}

        private bool ValueCheck()
        {{
            bool result = true;

            if (m{parm}ViewModel.{parm}Entity.RecordTime == null
                || ValidateUtil.IsBlank(m{parm}ViewModel.{parm}Entity.RecordTime.ToString()))
            {{
                MessageBox.Show(YcConstants.STR_ERROR_MSG_BLANK_VALUE, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                result = false;
            }}

            return result;
        }}
    }}
}}
    '''.format(module = prefix + moduleName, parm = moduleName)
    file = gPathAppControllers + os.path.sep + "SquadronLogbook" + os.path.sep + "Lb" + prefix + moduleName + "Controller.cs"
    fobj = open(file, 'w')
    fobj.write(content)
    fobj.close()  
    print ('{} file created!'.format("Lb" + prefix + moduleName + "Controller.cs"))

def updateAppService(prefix, moduleName):
    file = gPathAppServices + os.path.sep + "IEntityService.cs"
    fobj = codecs.open(file, 'rU', 'utf-8')
    
    content = fobj.read()

    fobj.close()

    content = content.replace('        yctrafficEntities Entities', \
                                 '        ObservableCollection<Zdtz{module}> Lb{parm}s {{ get; }}\r\n        IEnumerable<Zdtz{module}> EnumLb{parm}s {{ get; }}\r\n        IEnumerable<Zdtz{module}> Query{parm}s(int deptId, DateTime startDate, DateTime endDate);\r\n\r\n        yctrafficEntities Entities'.format(module = prefix + moduleName, parm = moduleName))

    fobj2 = codecs.open(file, 'w', 'utf-8')
    fobj2.write(content)
    fobj2.close()  
    print ('IEntityService.cs updated!')
    file2 = gPathAppServices + os.path.sep + "EntityService.cs"
    fobj3 = codecs.open(file2, 'rU', 'utf-8')
    
    content2 = fobj3.read()

    fobj3.close()

    content2 = content2.replace('        private ObservableCollection<HealthArchiveTable> healthArchives = null;', \
                                '''        private ObservableCollection<Zdtz{module}> lb{parm}s = null;\r
        private IEnumerable<Zdtz{module}> enumLb{parm}s = null;\r
\r
        private ObservableCollection<HealthArchiveTable> healthArchives = null;'''.format(module = prefix + moduleName, parm = moduleName))

    content2 = content2.replace('region SquadronLogbookProperties', \
                                '''region SquadronLogbookProperties\r
\r
        public ObservableCollection<Zdtz{module}> Lb{parm}s\r
        {{\r
            get\r
            {{\r
                if (lb{parm}s == null && entities != null)\r
                {{\r
                    lb{parm}s = new EntityObservableCollection<Zdtz{module}>(entities.Zdtz{module}s);\r
                }}\r
                return lb{parm}s;\r
            }}\r
        }}\r
\r
        public IEnumerable<Zdtz{module}> EnumLb{parm}s\r
        {{\r
            get\r
            {{\r
                if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTALL))\r
                {{\r
                    enumLb{parm}s = entities.Zdtz{module}s.Where\r
                    (\r
                        entity => entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE\r
                    );\r
\r
                }}\r
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))\r
                {{\r
                    List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;\r
                    if (depts != null && depts.Count > 0)\r
                    {{\r
                        var predicate = PredicateBuilder.False<Zdtz{module}>();\r
                        foreach (BaseOrganizeEntity dept in depts)\r
                        {{\r
                            BaseOrganizeEntity tempDept = dept;\r
                            predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);\r
                        }}\r
                        predicate = predicate.And(p => p.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE);\r
                        enumLb{parm}s = entities.Zdtz{module}s.AsExpandable().Where(predicate);\r
                    }}\r
                }}\r
                else if (CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_BROWSE))\r
                {{\r
                    int curDeptId = Convert.ToInt32(CurrentLoginService.Instance.CurrentUserInfo.DepartmentId);\r
                    enumLb{parm}s = entities.Zdtz{module}s.Where\r
                            (\r
                                entity =>\r
                                    ((entity.OwnDepartmentId == curDeptId || entity.OwnDepartmentId == YcConstants.INT_COMPANY_ID)\r
                                    && entity.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE)\r
                            );\r
                }}\r
                return enumLb{parm}s;\r
            }}\r
        }}\r
        '''.format(module = prefix + moduleName, parm = moduleName))

    content2 = content2.replace('region SquadronLogbookMember', \
                                '''region SquadronLogbookMember\r
\r
        public IEnumerable<Zdtz{module}> Query{parm}s(int deptId, DateTime startDate, DateTime endDate)\r
        {{\r
            IEnumerable<Zdtz{module}> queryResults = null;\r
\r
            var results = from u in entities.Zdtz{module}s\r
                            where u.RecordTime >= startDate && u.RecordTime <= endDate && u.IsDeleted == YcConstants.INT_DB_DATA_AVAILABLE\r
                            select u;\r
            if (deptId > 0)\r
            {{\r
                results = results.Where(p => p.OwnDepartmentId == deptId);\r
            }}\r
            else if (deptId == 0 && CurrentLoginService.Instance.IsAuthorized(YcConstants.STR_AUTH_LOGBOOK_LISTCHARGEDEPTS))\r
            {{\r
                List<BaseOrganizeEntity> depts = AuthService.Instance.FddZwChargeDepts;\r
                if (depts != null && depts.Count > 0)\r
                {{\r
                    var predicate = PredicateBuilder.False<Zdtz{module}>();\r
                    foreach (BaseOrganizeEntity dept in depts)\r
                    {{\r
                        BaseOrganizeEntity tempDept = dept;\r
                        predicate = predicate.Or(p => p.OwnDepartmentId == tempDept.Id);\r
                    }}\r
                    results = results.AsExpandable().Where(predicate);\r
                }}\r
\r
            }}\r
            queryResults = (IEnumerable<Zdtz{module}>)results.ToList();\r
\r
            return queryResults;\r
        }}\r
        '''.format(module = prefix + moduleName, parm = moduleName))

    fobj4 = codecs.open(file2, 'w', 'utf-8')
    fobj4.write(content2)
    fobj4.close()  
    print ('EntityService.cs updated!')

def updateAppCsproj(prefix, moduleName):
    file = gPathApplication + os.path.sep + "WafTraffic.Applications.csproj"
    fobj = codecs.open(file, 'rU', 'utf-8')
    
    content = fobj.read()

    fobj.close()

    content = content.replace('    <Compile Include="Properties\\AssemblyInfo.cs" />', \
                                 '    <Compile Include="Controllers\\SquadronLogbook\\Lb{module}Controller.cs" />\r\n    <Compile Include="Properties\\AssemblyInfo.cs" />'.format(module = prefix + moduleName))
    content = content.replace('ViewModel.cs" />\r\n    <Compile Include="Views', \
                              'ViewModel.cs" />\r\n    <Compile Include="ViewModels\\SquadronLogbook\\Lb{module}UpdateViewModel.cs" />\r\n    <Compile Include="ViewModels\\SquadronLogbook\\Lb{module}QueryViewModel.cs" />\r\n    <Compile Include="Views'.format(module = prefix + moduleName))
    content = content.replace('View.cs" />\r\n  </ItemGroup>', \
                              'View.cs" />\r\n    <Compile Include="Views\\SquadronLogbook\\ILb{module}UpdateView.cs" />\r\n    <Compile Include="Views\\SquadronLogbook\\ILb{module}QueryView.cs" />\r\n  </ItemGroup>'.format(module = prefix + moduleName))

    fobj2 = codecs.open(file, 'w', 'utf-8')
    fobj2.write(content)
    fobj2.close()  
    print ('WafTraffic.Applications.csproj updated!')    

def codeGenerator():

    if not pathPrecheck():
        print ("Wrong path. Please make sure CodeGen.py is under WafTraffic folder.")
        return

    # get module name  
    while True:
        prefix = input('Enter prifix: ')
        moduleName = input('Enter module name: ')
        if fileCreatedCheck(prefix + moduleName):
            print ("ERROR: '%s' has already been generated" % moduleName)
        else:  
            break  

    createPresUpdateViews(prefix, moduleName)
    createPresQueryViews(prefix, moduleName)
    updatePresCsproj(prefix, moduleName)
    createAppViews(prefix, moduleName)
    createAppViewModels(prefix, moduleName)
    createController(prefix, moduleName)
    updateAppCsproj(prefix, moduleName)
    updateAppService(prefix, moduleName)
    return

if __name__ == "__main__":
    codeGenerator()
