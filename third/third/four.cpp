#include <iostream>
#include <thread>
#include <string>
#include <chrono>
#include <mutex>

using namespace std;

string ta = "�¼���";
mutex ta_m;


void Prop(string name, string hname)
{
	cout << name << ";" << hname << "����" << endl;
	cout << name << ";" << hname << "����" << endl;
	this_thread::sleep_for(std::chrono::milliseconds(1000));
}

void Gl(string name) {

	ta_m.lock();
	ta = name;
	Prop("��", ta);
	this_thread::sleep_for(std::chrono::milliseconds(5000));
	cout << "�� :" << "������" << endl;
	ta_m.unlock();
}

void main() 
{
	thread na_seok_hoon(Gl, "����");
	thread oh_jeong_seok(Gl, "����");

	na_seok_hoon.join();
	oh_jeong_seok.join();
}