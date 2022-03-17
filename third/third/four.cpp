#include <iostream>
#include <thread>
#include <string>
#include <chrono>
#include <mutex>

using namespace std;

string ta = "태수야";
mutex ta_m;


void Prop(string name, string hname)
{
	cout << name << ";" << hname << "진심" << endl;
	cout << name << ";" << hname << "몰루" << endl;
	this_thread::sleep_for(std::chrono::milliseconds(1000));
}

void Gl(string name) {

	ta_m.lock();
	ta = name;
	Prop("수", ta);
	this_thread::sleep_for(std::chrono::milliseconds(5000));
	cout << "수 :" << "끝내자" << endl;
	ta_m.unlock();
}

void main() 
{
	thread na_seok_hoon(Gl, "정석");
	thread oh_jeong_seok(Gl, "석훈");

	na_seok_hoon.join();
	oh_jeong_seok.join();
}