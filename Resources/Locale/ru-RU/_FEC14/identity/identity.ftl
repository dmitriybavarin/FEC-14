fec-identity-unknown = { $gender ->
    [female] { $age ->
        [young] молодая женщина
        [old] пожилая женщина
       *[other] женщина средних лет
    }
    [male] { $age ->
        [young] молодой мужчина
        [old] пожилой мужчина
       *[other] мужчина средних лет
    }
   *[other] { $age ->
        [young] молодой человек
        [old] пожилой человек
       *[other] человек средних лет
    }
}
fec-identity-unknown-job = {fec-identity-unknown}, {$job}

fec-fmt-list-and-two = {" и "}
fec-fmt-list-and-last = {" и "}
fec-fmt-list-or = {" или "}
