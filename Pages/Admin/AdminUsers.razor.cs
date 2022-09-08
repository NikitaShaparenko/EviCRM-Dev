namespace EviCRM.Server.Pages.Admin
{
    public partial class AdminUsers
    {
        void ResetPassword(int param) //Сброс пароля
        {
            memory_username = users_dt_fullname[param];
            memory_id = users_dt_id[param];
            memory_id_arr = param;
            OpenModal();
        }

        async Task OnHandlerResetPassword(string password)
        {
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.updateUserPasswordByID(memory_id, password));
            users_dt_passwords[memory_id_arr] = password;
            CloseModal();
            await InvokeAsync(StateHasChanged);

        }

        void OpenModal()
        {
            isModalOpened = true;
            StateHasChanged();
        }

        void CloseModal()
        {
            isModalOpened = false;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            division_dt = await mysqlc.getListUsersDivisionAsync();
            users_dt_id = await mysqlc.getListUsersIDAsync();
            users_dt_login = await mysqlc.getListUsersLoginAsync();
            users_dt_email = await mysqlc.getListUsersEmailAsync();
            users_dt_passwords = await mysqlc.getListUsersPasswordAsync();
            users_dt_position = await mysqlc.getListUsersPositionsAsync();
            users_dt_division = await mysqlc.getListUsersDivisionAsync();
            users_dt_fullname = await mysqlc.getListUsersFullnameAsync();
            users_dt_flagActivated = await mysqlc.getListUsersFlagActivatedAsync();
            users_dt_role = await mysqlc.getListUsersRoleAsync();

            ready = true;
            await InvokeAsync(StateHasChanged);
        }

        async Task ReDraw()
        {
            ready = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(150);

            division_dt.Clear();
            users_dt_id.Clear();
            users_dt_login.Clear();
            users_dt_email.Clear();
            users_dt_passwords.Clear();
            users_dt_position.Clear();
            users_dt_division.Clear();
            users_dt_fullname.Clear();
            users_dt_flagActivated.Clear();
            users_dt_role.Clear();

            division_dt = await mysqlc.getListUsersDivisionAsync();
            users_dt_id = await mysqlc.getListUsersIDAsync();
            users_dt_login = await mysqlc.getListUsersLoginAsync();
            users_dt_email = await mysqlc.getListUsersEmailAsync();
            users_dt_passwords = await mysqlc.getListUsersPasswordAsync();
            users_dt_position = await mysqlc.getListUsersPositionsAsync();
            users_dt_division = await mysqlc.getListUsersDivisionAsync();
            users_dt_fullname = await mysqlc.getListUsersFullnameAsync();
            users_dt_flagActivated = await mysqlc.getListUsersFlagActivatedAsync();
            users_dt_role = await mysqlc.getListUsersRoleAsync();
            ready = true;
            await InvokeAsync(StateHasChanged);
        }



        public async Task Activate(string id)
        {
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.updateUserActivationByID(id, "1"));
            await ReDraw();
            await InvokeAsync(StateHasChanged);
        }

        public async Task DowngradeToUser(string id)
        {
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.updateUserRoleByID(id, "user"));
            await ReDraw();
            await InvokeAsync(StateHasChanged);
        }

        public async Task UpgradeToAdmin(string id)
        {
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.updateUserRoleByID(id, "admin"));
            await ReDraw();
            await InvokeAsync(StateHasChanged);
        }


        public async Task Deactivate(string id)
        {
            await mysqlc.MySql_ContextAsyncGeneral(mysqlc.updateUserActivationByID(id, "0"));
            await ReDraw();
            await InvokeAsync(StateHasChanged);
        }
    }
}
