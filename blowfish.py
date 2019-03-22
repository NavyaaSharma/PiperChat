from Crypto.Cipher import Blowfish
from struct import pack


def encrypt(key, text):
    key = key.encode()
    text = text.encode()
    b = Blowfish.block_size
    cipher = Blowfish.new(key, Blowfish.MODE_CBC)
    plen = b-len(text)%b
    padding = [plen]*plen
    padding = pack('b'*plen, *padding)
    msg = cipher.iv + cipher.encrypt(text + padding)
    return msg



def decrypt(key, text):
    key = key.encode()
    cipher = Blowfish.new(key, Blowfish.MODE_CBC)
    return cipher.decrypt(text)


text = input('enter data')
key = input('enter key')
ldata = list(text)
ldata.insert(0, '$$$')
ldata.append('$$$')
data = ''.join(ldata)

encrypted = encrypt(key, data)
print(encrypted)

decrypted = decrypt(key, encrypted)
decrypted = str(decrypted)

for i in range(0, len(decrypted)):
    if decrypted[i:i+3] == '$$$':
        front = i
        break
for j in range(0, len(decrypted)):
    if decrypted[j:j+3] == '$$$':
        rear = j
print(decrypted[front+3:rear])













