from socket import *
from mailer import Mailer
import sqlite3
import random

Email = ""


def sign_up():
    """

    This function is responsible for signing up accounts
    and adding them to the Database. The function also sends & validates
    the verification code and input integrity.

    :argument:
    None

    :return:
    Returns page transference & messages in accordance to the user.

    """

    global Email

    while True:
        account = client_soc.recv(256).decode().split(',')
        page = account[0]
        print(page)

        if page == "ToLogIn":
            return "ToLogIn"

        elif page == "SignUp":
            print("Client Page: " + page)
            FirstName = account[1]
            LastName = account[2]
            Email = account[3]
            Password = account[4]

            print("Client: account: " + FirstName, LastName, Email, Password)

            if not (len(FirstName) == 0 or len(LastName) == 0 or len(Email) == 0 or len(Password) == 0):
                cur.execute("Select Email from Accounts Where Email='" + Email + "'")
                data = cur.fetchall()

                if len(data) == 0:
                    client_soc.sendall("CanSignUp".encode())
                    code = random.randint(100000, 1000000)

                    print("Server code: " + str(code))

                    mail = Mailer(email='', password='')
                    mail.send(receiver=Email,
                              subject='iPulse Account Verification',
                              message='Hi ' + FirstName + ', Your authentication code is: ' + str(code))
                    
                    clientCode = client_soc.recv(256).decode().split(",")

                    print("Client: code: " + clientCode[0])

                    if clientCode[0] != "BackToSignUp":
                        while clientCode[0] != str(code):
                            client_soc.sendall("Wrong code".encode())
                            clientCode = client_soc.recv(256).decode().split(",")
                            print("Client: code: " + clientCode[0])

                        sql = '''INSERT INTO Accounts(FirstName,LastName,Email,Password)
                                                                                         VALUES(?,?,?,?) '''

                        cur.execute(sql, (FirstName, LastName, Email, Password))
                        conn.commit()
                        client_soc.sendall("Account Created".encode())
                        print("Account Created")

                        return "HeartPage"
                    else:
                        print("BackToSignUp")
                        pass
                else:
                    client_soc.sendall("Account already exists!".encode())
                    print("Account already exists!")
            elif page == "BackToMain":
                return page
            else:
                client_soc.sendall("You must fill all the fields.".encode())
                print("You must fill all the fields.")
        elif page == "BackToMain":
            return page


def log_in():
    """

    This function is responsible for logging in existing accounts
    and verifying them from the Database. The function also sends & validates
    the verification code and input integrity.

    :argument:
    None

    :return:
    Returns page transference & messages in accordance to the user.

    """

    global Email
    account = client_soc.recv(256).decode().split(',')
    page = account[0]
    print(page)

    if page == "ToSignUp":
        return "ToSignUp"

    elif page == "ResetPass":
        reset_password(page)
        page = "Login"

    elif page == "Login":
        print("Client Page: " + page)
        Email = account[1]
        Password = account[2]

        if not (len(Email) == 0 or len(Password) == 0):
            cur.execute("Select Email from Accounts Where Email='" + Email + "'")
            fetchall = cur.fetchall()

            if len(fetchall) > 0:
                EmailDB = str(fetchall[0])[2:-3]
            else:
                EmailDB = ""

            cur.execute("Select Password from Accounts Where Email='" + Email + "'")

            fetchall = cur.fetchall()
            if len(fetchall) > 0:
                PasswordDB = str(fetchall[0])[2:-3]
            else:
                PasswordDB = ""

            print("Client: account: " + Email, Password)
            print("DB: account: " + EmailDB, PasswordDB)

            cur.execute("Select Email from Accounts Where Email='" + Email + "'")
            data = cur.fetchall()

            if Email == EmailDB and Password == PasswordDB:
                print("Account is verified")
                cur.execute("Select FirstName from Accounts Where Email='" + Email + "'")
                FirstName = cur.fetchall()[0]
                cur.execute("Select LastName from Accounts Where Email='" + Email + "'")
                LastName = cur.fetchall()[0]
                client_soc.sendall("CanLogin".encode())
                code = random.randint(100000, 1000000)
                print("Server code: " + str(code))

                mail = Mailer(email='', password='')
                mail.send(receiver=Email,
                          subject='iPulse Sign In Code',
                          message='Hi ' + FirstName[0] + " " + LastName[0] + ','
                                                                             ' Please Enter the code in order to access your iPulse account :)'
                                                                             ' Your authentication code is: ' + str(
                              code))
                clientCode = client_soc.recv(256).decode().split(",")

                print("Client: code: " + clientCode[0])

                while clientCode[0] != str(code):
                    if clientCode[0] != "BackToLogin":
                        client_soc.sendall("Wrong code".encode())
                        clientCode = client_soc.recv(256).decode().split(",")
                        print("Client: code: " + clientCode[0])
                    else:
                        return "BackToLogin"

                client_soc.sendall("Account Verified".encode())
                print("Account Verified")
                return "HeartScreen"
            else:
                client_soc.sendall("Account don't match".encode())
                print("Account don't match")
        else:
            client_soc.sendall("You must fill all the fields.".encode())
            print("You must fill all the fields.")
    elif page == "BackToMain":
        return page


def reset_password(page):
    """

    This function is responsible for updating passwords for existing accounts
    and verifying them from the Database. The function also sends & validates
    the verification code and input integrity.

    :argument:
    Page
`
    :return:
    Returns page transference & messages in accordance to the user.

    """

    global Email

    print("Client Page: " + page)

    Email = ""
    while Email != "BackToLogin":
        Email = client_soc.recv(256).decode()
        if Email != "BackToLogin":
            if len(Email) > 0:
                cur.execute("Select Email from Accounts Where Email='" + Email + "'")
                fetchall = cur.fetchall()

                if len(fetchall) > 0:
                    EmailDB = str(fetchall[0])[2:-3]
                else:
                    EmailDB = ""

                print("Client Email: " + Email)
                print("DB: Email: " + EmailDB)

                if Email == EmailDB:
                    client_soc.sendall("Email verified".encode())
                    print("Email verified")
                    code = random.randint(100000, 1000000)
                    print("Server code: " + str(code))
                    mail = Mailer(email='', password='')
                    mail.send(receiver=Email,
                              subject='iPulse Sign In Code',
                              message='Hi! Please enter the verification code in order to change your password. '
                                      'Code: ' + str(code))

                    clientCode = client_soc.recv(256).decode().split(",")
                    print("Client: code: " + clientCode[0])

                    while clientCode[0] != str(code):
                        if clientCode[0] != "BackToResetPass":
                            client_soc.sendall("Wrong code".encode())
                            clientCode = client_soc.recv(256).decode().split(",")
                            print("Client: code " + clientCode[0])
                        else:
                            clientCode[0] = "BackToResetPass"
                            break

                    if clientCode[0] != "BackToResetPass":
                        client_soc.sendall("Account Verified".encode())
                        password = client_soc.recv(256).decode()

                        if password != "BackToResetPass":
                            print("Client Page: " + password)

                            if len(password) > 0:
                                cur.execute(
                                    "UPDATE Accounts SET Password = '" + password + "' WHERE Email = '" + Email + "'")
                                conn.commit()
                                client_soc.sendall("Password Updated".encode())
                                print("Password Updated")
                                return "Login"
                else:
                    client_soc.sendall("Email is not registered".encode())
                    print("Email is not registered")
            else:
                print("You must fill all the fields")


def heart_screen():
    """

    This function is responsible for the page transference in the Heart Screen.

    :argument:
    None

    :return:
    Returns page transference in accordance to the user.

    """

    global Email

    page = client_soc.recv(256).decode()
    print("Client Page: " + page)

    if page == "BackToSelection":
        return "BackToMain"

    elif page == "AccountScreen":
        print("Client Page: " + page)
        account_details()


def account_details():

    """

    This function is responsible for getting the Email & the Full name
    of the user from the Database and send it to the client.

    :argument:
    None

    :return:
    Returns account details in accordance to the user.

    """

    global Email

    conn = sqlite3.connect("iPulseDB.db")
    cur = conn.cursor()
    cur.execute("Select FirstName from Accounts Where Email='" + Email + "'")
    FirstName = str(cur.fetchall()[0])[2:-3]
    cur.execute("Select LastName from Accounts Where Email='" + Email + "'")
    LastName = str(cur.fetchall()[0])[2:-3]
    account_details = Email + "," + FirstName + "," + LastName
    client_soc.sendall(account_details.encode())


while True:
    try:
        print("Server started")
        conn = sqlite3.connect("iPulseDB.db")
        cur = conn.cursor()
        LISTEN_PORT = 0000
        listening_sock = socket(AF_INET, SOCK_STREAM)
        server_address = ("0.0.0.0", LISTEN_PORT)
        listening_sock.bind(server_address)
        listening_sock.listen(1)
        client_soc, client_address = listening_sock.accept()
        client_soc.sendall("Connected".encode())
        msg = client_soc.recv(256).decode()
        print("Client: " + msg)

        while True:
            print("Client Page: Page Selection")
            data = client_soc.recv(256).decode()
            while True:
                if data == 'Signup' or data[0] == 'SignUp':
                    print("Client Page: " + data)
                    retVar = sign_up()

                    if retVar == "ToLogIn":
                        data = 'Login'

                    elif retVar == "BackToMain":
                        break

                    elif retVar == "HeartPage":
                        data = "HeartScreen"

                if data == 'Login' or data[0] == 'Login':
                    print("Client Page: " + data)
                    retVar = log_in()

                    if retVar == "ToSignUp":
                        data = 'Signup'
                    elif retVar == "BackToMain":
                        break
                    elif retVar == "HeartScreen":
                        data = "HeartScreen"
                    elif retVar == "BackToLogin":
                        data = "Login"
                elif data == "HeartScreen":
                    while data == "HeartScreen":
                        print("Client Page: " + data)
                        retVar = heart_screen()

                        if retVar == "BackToMain":
                            data = "BackToMain"
                elif data == "BackToMain":
                    break


    except ConnectionError as e:
        print("Client Disconnected")
        print("Server Closed")
