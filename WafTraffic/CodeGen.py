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

def pathPrecheck():    
    return os.path.exists(gPathPresentation) \
           and os.path.exists(gPathApplication) \
           and os.path.exists(gPathDomain)

def fileCreatedCheck(moduleName):
    xamlFile = gPathPresViews + os.path.sep + moduleName + "View.xaml"
    return os.path.exists(xamlFile)

def createPresViews(moduleName):
    xamlContent = '''<UserControl x:Class="WafTraffic.Presentation.Views.{}View"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:themes="clr-namespace:WPF.Themes;assembly=WPF.Themes"
             xmlns:xctk="http://schemas.xceed.com/wpf/xaml/toolkit"
             d:DesignHeight="300"
             d:DesignWidth="300"
             FontFamily="Microsoft YaHei"
             themes:ThemeManager.Theme="WhistlerBlue"
             mc:Ignorable="d">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="180" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>

    </Grid>
</UserControl>
    '''.format(moduleName)
    # create xaml file under .\WafTraffic\WafTraffic.Presentation\Views
    xamlFile = gPathPresViews + os.path.sep + moduleName + "View.xaml"
    fobj = open(xamlFile, 'w')
    fobj.write(xamlContent)
    fobj.close()  
    print ('{} file created!'.format(moduleName + "View.xaml"))

    xamlCsContent = '''using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Waf.Applications;
using System.Windows.Controls;
using WafTraffic.Domain;
using WafTraffic.Applications.ViewModels;
using WafTraffic.Applications.Views;

namespace WafTraffic.Presentation.Views
{{
    /// <summary>
    /// {module}View.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(I{module}View))]
    public partial class {module}View  : UserControl, I{module}View
    {{
        public {module}View()
        {{
            InitializeComponent();
        }}
    }}
}}
    '''.format(module = moduleName)
    # create xaml.cs file under .\WafTraffic\WafTraffic.Presentation\Views
    xamlCsFile = gPathPresViews + os.path.sep + moduleName + "View.xaml.cs"
    fobj2 = open(xamlCsFile, 'w')
    fobj2.write(xamlCsContent)
    fobj2.close()  
    print ('{} file created!'.format(moduleName + "View.xaml.cs"))

def updatePresCsproj(moduleName):
    file = gPathPresentation + os.path.sep + "WafTraffic.Presentation.csproj"
    fobj = codecs.open(file, 'rU', 'utf-8')
    
    content = fobj.read()

    fobj.close()

    content = content.replace('</Compile>\r\n    <Page', \
                                 '</Compile>\r\n    <Compile Include="Views\\{module}View.xaml.cs">\r\n\
      <DependentUpon>{module}View.xaml</DependentUpon>\r\n    </Compile>\r\n    <Page'.format(module = moduleName))
    content = content.replace('</Page>\r\n  </ItemGroup>', \
                              '</Page>\r\n    <Page Include="Views\\{module}View.xaml">\r\n\
      <Generator>MSBuild:Compile</Generator>\r\n      <SubType>Designer</SubType>\r\n\
    </Page>\r\n  </ItemGroup>'.format(module = moduleName))

    fobj2 = codecs.open(file, 'w', 'utf-8')
    fobj2.write(content)
    fobj2.close()  
    print ('WafTraffic.Presentation.csproj updated!')
    return

def createAppViews(moduleName):
    content = '''using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Waf.Applications;

namespace WafTraffic.Applications.Views
{{
    public interface I{module}View : IView
    {{
    }}
}}
    '''.format(module = moduleName)
    file = gPathAppViews + os.path.sep + "I" + moduleName + "View.cs"
    fobj = open(file, 'w')
    fobj.write(content)
    fobj.close()  
    print ('{} file created!'.format("I" + moduleName + "View.cs"))

def createAppViewModels(moduleName):
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

namespace WafTraffic.Applications.ViewModels
{{
    [Export]
    public class {module}ViewModel : ViewModel<I{module}View>
    {{
        [ImportingConstructor]
        public {module}ViewModel(I{module}View view, IEntityService entityservice)
            : base(view)
        {{

        }}
    }}
}}
    '''.format(module = moduleName)
    file = gPathAppViewModels + os.path.sep + moduleName + "ViewModel.cs"
    fobj = open(file, 'w')
    fobj.write(content)
    fobj.close()  
    print ('{} file created!'.format(moduleName + "ViewModel.cs"))    

def createController(moduleName):
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

namespace WafTraffic.Applications.Controllers
{{
    [Export]
    internal class {module}Controller : Controller
    {{
        private readonly CompositionContainer container;
        private readonly IShellService shellService;
        private readonly System.Waf.Applications.Services.IMessageService messageService;
        private readonly IEntityService entityService;

        private MainFrameViewModel mainFrameViewModel;

        private readonly DelegateCommand newCommand;
        private readonly DelegateCommand modifyCommand;
        private readonly DelegateCommand deleteCommand;
        private readonly DelegateCommand saveCommand;

        [ImportingConstructor]
        public {module}Controller(CompositionContainer container, System.Waf.Applications.Services.IMessageService messageService, IShellService shellService, IEntityService entityService)
        {{
            this.container = container;
            this.messageService = messageService;
            this.shellService = shellService;
            this.entityService = entityService;

            mainFrameViewModel = container.GetExportedValue<MainFrameViewModel>();

            this.newCommand = new DelegateCommand(() => NewOper(), null);
            this.modifyCommand = new DelegateCommand(() => ModifyOper(), null);
            this.deleteCommand = new DelegateCommand(() => DeleteOper(), null);
            this.saveCommand = new DelegateCommand(() => Save(), null);
        }}

        public void Initialize()
        {{

        }}

        public bool NewOper()
        {{
            bool newer = true;

            return newer;
        }}

        public bool ModifyOper()
        {{
            bool newer = true;

            return newer;
        }}

        public bool DeleteOper()
        {{            
            bool newer = true;


            return newer;
        }}

        public bool Save()
        {{
            bool saved = false;

            try
            {{

            }}
            catch (ValidationException e)
            {{
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidEntities, e.Message));
            }}
            catch (UpdateException e)
            {{
                messageService.ShowError(shellService.ShellView, string.Format(CultureInfo.CurrentCulture,
                    WafTraffic.Applications.Properties.Resources.SaveErrorInvalidFields, e.InnerException.Message));
            }}

            return saved;
        }}

    }}
}}
    '''.format(module = moduleName)
    file = gPathAppControllers + os.path.sep + moduleName + "Controller.cs"
    fobj = open(file, 'w')
    fobj.write(content)
    fobj.close()  
    print ('{} file created!'.format(moduleName + "Controller.cs"))

def updateAppCsproj(moduleName):
    file = gPathApplication + os.path.sep + "WafTraffic.Applications.csproj"
    fobj = codecs.open(file, 'rU', 'utf-8')
    
    content = fobj.read()

    fobj.close()

    content = content.replace('    <Compile Include="Properties\\AssemblyInfo.cs" />', \
                                 '    <Compile Include="Controllers\\{module}Controller.cs" />\r\n    <Compile Include="Properties\\AssemblyInfo.cs" />'.format(module = moduleName))
    content = content.replace('ViewModel.cs" />\r\n    <Compile Include="Views', \
                              'ViewModel.cs" />\r\n    <Compile Include="ViewModels\\{module}ViewModel.cs" />\r\n    <Compile Include="Views'.format(module = moduleName))
    content = content.replace('View.cs" />\r\n  </ItemGroup>', \
                              'View.cs" />\r\n    <Compile Include="Views\\I{module}View.cs" />\r\n  </ItemGroup>'.format(module = moduleName))

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
        moduleName = input('Enter module name: ')
        if fileCreatedCheck(moduleName):
            print ("ERROR: '%s' has already been generated" % moduleName)
        else:  
            break  
    
    createPresViews(moduleName)
    updatePresCsproj(moduleName)
    createAppViews(moduleName)
    createAppViewModels(moduleName)
    createController(moduleName)
    updateAppCsproj(moduleName)
    return

if __name__ == "__main__":
    codeGenerator()
