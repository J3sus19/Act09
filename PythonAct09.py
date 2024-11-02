import tkinter as tk
from tkinter import messagebox
import serial
import time
import mysql.connector

arduino_port = "COM4"
baud_rate = 9600
arduino = None

db_config = {
    'host': 'localhost',
    'database': 'Formulario',
    'user': 'root',
    'password': '1993'
}

def conectar():
    global arduino
    try:
        arduino = serial.Serial(arduino_port, baud_rate)
        time.sleep(2)
        lbConexion.config(text="Estado: Conectado", fg="green")
        messagebox.showinfo("Conexión", "Conexión establecida.")
       
    except serial.SerialException:
        messagebox.showerror("Error", "No se pudo conectar al Arduino. Verifique el puerto.")

def desconectar():
    global arduino
    if arduino and arduino.is_open:
        arduino.close()
        lbConexion.config(text="Estado: Desconectado", fg="red")
        messagebox.showinfo("Conexión", "Conexión terminada.")
    else:
        messagebox.showwarning("Advertencia", "No hay conexión activa.")

def enviar_limite():
    global arduino
    if arduino and arduino.is_open:
        try:
            limite = tblimTemp.get()
            if limite.isdigit():
                arduino.write(f"{limite}\n".encode())
                messagebox.showinfo("Enviado", f"Límite de temperatura enviado: {limite}°C")
            else:
                messagebox.showerror("Error", "Ingrese un valor numérico para el límite")
        except Exception as e:
            messagebox.showerror("Error", f"No se pudo enviar el límite: {e}")
    else:
        messagebox.showwarning("Advertencia", "Conéctese al Arduino antes de enviar el límite.")

def read_from_arduino():
    global arduino
    while arduino and arduino.is_open:
        try:
            data = arduino.readline().decode().strip()
            if "Temperatura" in data:
                temp_value = data.split(":")[1].strip().split(" ")[0]
                lbTemp.config(text=f"Temperatura: {temp_value} °C")
                insertar_datos_en_bd(temp_value)
            time.sleep(1)
        except Exception as e:
            print(f"Error leyendo datos: {e}")
            break

def insertar_datos_en_bd(temperatura):
    try:
        connection = mysql.connector.connect(**db_config)
        cursor = connection.cursor()
        insert_query = "INSERT INTO dattemp (temperatura) VALUES (%s)"
        cursor.execute(insert_query, (temperatura,))
        connection.commit()
        cursor.close()
        connection.close()
    except mysql.connector.Error as err:
        messagebox.showerror("Error en la base de datos", f"Error al insertar datos: {err}")


root = tk.Tk()
root.title("Control de Arduino")

lbConexion = tk.Label(root, text="Estado: Desconectado", fg="red")
lbConexion.pack(pady=10)

lbTemp = tk.Label(root, text="Temperatura: --- °C")
lbTemp.pack(pady=10)

tblimTemp = tk.Entry(root)
tblimTemp.pack(pady=10)
tblimTemp.insert(0, "Ingrese límite de temperatura")

btnConectar = tk.Button(root, text="Conectar", command=conectar)
btnConectar.pack(pady=5)

btnDesconectar = tk.Button(root, text="Desconectar", command=desconectar)
btnDesconectar.pack(pady=5)

btnEnviar = tk.Button(root, text="Enviar límite", command=enviar_limite)
btnEnviar.pack(pady=5)

root.mainloop()
