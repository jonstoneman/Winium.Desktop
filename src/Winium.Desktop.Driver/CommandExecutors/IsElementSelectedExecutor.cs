﻿namespace Winium.Desktop.Driver.CommandExecutors
{
    #region using

    using System.Windows.Automation;

    using Winium.Cruciatus.Exceptions;
    using Winium.Cruciatus.Extensions;
    using Winium.StoreApps.Common;

    #endregion

    internal class IsElementSelectedExecutor : CommandExecutorBase
    {
        #region Methods

        protected override string DoImpl()
        {
            var registeredKey = this.ExecutedCommand.Parameters["ID"].ToString();

            var element = this.Automator.Elements.GetRegisteredElement(registeredKey);

            var isSelected = false;

            try
            {
                var isSelectedItemPattrenAvailable =
                    element.GetAutomationPropertyValue<bool>(AutomationElement.IsSelectionItemPatternAvailableProperty);

                if (isSelectedItemPattrenAvailable)
                {
                    var selectionItemProperty = SelectionItemPattern.IsSelectedProperty;
                    isSelected = element.GetAutomationPropertyValue<bool>(selectionItemProperty);
                }
            }
            catch (CruciatusException)
            {
                var toggleStateProperty = TogglePattern.ToggleStateProperty;
                var toggleState = element.GetAutomationPropertyValue<ToggleState>(toggleStateProperty);

                isSelected = toggleState == ToggleState.On;
            }

            return this.JsonResponse(ResponseStatus.Success, isSelected);
        }

        #endregion
    }
}
