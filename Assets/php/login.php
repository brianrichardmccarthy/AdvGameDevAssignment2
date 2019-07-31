<?php

    $host = "localhost";
    $db = "id8972992_assignment2";
    $password = "T0!zmECTXB6Wi8f&";
    $name = "id8972992_20063914";
    
    $username = $_GET["name"];
    $userpassword = $_GET["password"];

    if ($username != null && $userpassword != null) {
        $conn = new mysqli($host, $name, $password, $db);
        if ($conn->connect_error) {
            die("Connection failed: " . $conn->connect_error);
        }

        $query = "SELECT * FROM users where username = '$username' and password = '$userpassword' limit 1;";
        $result = $conn->query($query);

        if ($result->num_rows > 0) {
            $result = $conn->query($query);
            $row = $result->fetch_assoc();
            echo $row["score"] . "," . $row["level"];
        }


        $conn->close();
    }

?>