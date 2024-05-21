import cv2
from cvzone.HandTrackingModule import HandDetector
import socket

# Parameters
width, height = 1280, 720

# Capture web cam
cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

# Hand Detector
detector = HandDetector(maxHands=2, detectionCon=0.8)

# Communication
sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)

while True:
    # Get the frame from webcam
    success, img = cap.read()

    # Hands
    hands, img = detector.findHands(img)

    # Transfered data
    data = []

    if hands:
        # Get the first hand detected
        hand = hands[0]

        # Get the landmark list
        lmlist = hand['lmList']

        for lm in lmlist:
            data.extend([lm[0],height-lm[1],lm[2]])

        data.extend([sum(detector.fingersUp(hand))])

        if len(hands) >= 2:
             # Get the second hand detected
            hand = hands[1]

            # Get the landmark list
            lmlist = hand['lmList']

            for lm in lmlist:
                data.extend([lm[0],height-lm[1],lm[2]])
            
            data.extend([sum(detector.fingersUp(hand))])

    else:
        data = [0]
    # Send data
    sock.sendto(str.encode(str(data)), serverAddressPort)



    # img = cv2.resize(img, (0,0), None, 0.5, 0.5)
    # img = cv2.flip(img, 1)
    # cv2.imshow("Image", img)
    # cv2.waitKey(1)