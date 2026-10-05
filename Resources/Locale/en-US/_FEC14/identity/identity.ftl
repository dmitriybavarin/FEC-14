fec-identity-unknown = { $gender ->
   *[other] { $age ->
       *[other] {$ageString} {$genderString}
    }
}
fec-identity-unknown-job = { $gender ->
   *[other] { $age ->
       *[other] {$ageString} {$job} {$genderString}
    }
}

fec-fmt-list-and-two = {" and "}
fec-fmt-list-and-last = {", and "}
fec-fmt-list-or = {" or "}
