Imports System.Security.Cryptography

Friend Class Simple3des
    Private TripleDes As New TripleDESCryptoServiceProvider
    Private Function TruncateHash(ByVal key As String, length As Integer) As Byte()
        Dim sha512 As New SHA512CryptoServiceProvider
        Dim keybytes() As Byte = System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha512.ComputeHash(keybytes)
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

    Sub New(key As String)
        TripleDes.Key = TruncateHash(key, TripleDes.KeySize / 8)
        TripleDes.IV = TruncateHash(key, TripleDes.BlockSize / 8)
    End Sub
    Public Function EncryptData(
        ByVal plaintext As String) As String


        Dim plaintextBytes() As Byte = System.Text.Encoding.Unicode.GetBytes(plaintext)
        Dim ms As New System.IO.MemoryStream

        Dim encStream As New CryptoStream(ms,
            TripleDes.CreateEncryptor(),
            System.Security.Cryptography.CryptoStreamMode.Write)

        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        Return Convert.ToBase64String(ms.ToArray)
    End Function
    Public Shared Function EncodeEnc(ByVal txt As String) As String
        Dim plainText As String = txt
        Dim password As String = "piperchat"
        Dim wrapper As New Simple3des(password)
        Dim cipherText As String = wrapper.EncryptData(plainText)
        Return cipherText

    End Function
    Public Function DecryptData(
 ByVal encryptedtext As String) As String
        Try

            Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

            Dim ms As New System.IO.MemoryStream

            Dim decStream As New CryptoStream(ms,
                TripleDes.CreateDecryptor(),
                System.Security.Cryptography.CryptoStreamMode.Write)

            decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
            decStream.FlushFinalBlock()

            Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
        Catch ex As Exception
        End Try

    End Function

    Public Shared Function DecodeDEC(ByVal cipherText As String) As String

        Dim password As String = "piperchat"
        Dim wrapper As New Simple3des(password)
        Try
            Dim plainText As String = wrapper.DecryptData(cipherText)
            Return plainText
        Catch ex As System.Security.Cryptography.CryptographicException
            MsgBox("The data could not be decrypted with the password.")
        End Try
    End Function
End Class
