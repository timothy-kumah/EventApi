﻿namespace EventApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public string CogNome { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

    }
}
