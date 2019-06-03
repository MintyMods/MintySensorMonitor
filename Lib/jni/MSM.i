/* File: MSM.i */
%module MSMModule
%include "std_string.i"
%{
#include "MSM.h"
using namespace mintymods;
using namespace std;
typedef std::string String;
%}

class MSM {
public:
  ~MSM();
  std::string getJson(const std::string request);
};


