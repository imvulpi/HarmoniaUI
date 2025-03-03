#include "commons/string_helper.h"

String replace(String str, String replace, String to)
{
    String result;
    unsigned int str_length = str.length();
    unsigned int replace_length = replace.length();
    unsigned int index = 0;
    for (unsigned int i = 0; i < str_length; i++)
    {
        for (size_t o = 0; o < replace_length; o++)
        {
            if (str_length - i >= replace_length)
            {
                if (str[i + index] == replace[o])
                {
                    index += 1;
                }
                else
                {
                    index = 0;
                    break;
                }
            }
            else
            {
                index = 0;
                break;
            }
        }

        if (index == replace_length)
        {
            result += to;
            i += index - 1; // -1 because it doesnt count this iteration which is also a part of replaced string.
            index = 0;
        }
        else
        {
            result += str[i];
        }
    }
    return result;
}

List<String> split(String str, String seperator, bool ignore_empty)
{

    List<String> result;
    unsigned int str_length = str.length();
    unsigned int seperator_length = seperator.length();
    unsigned int seperator_match = 0;

    String current_split;
    for (unsigned int i = 0; i < str_length; i++)
    {
        if (str[i + seperator_match] == seperator[seperator_match])
        {
            seperator_match++;
        }
        else
        {
            if (seperator_match > 0)
            {
                for (size_t o = 0; o < seperator_match; o++)
                {
                    current_split += seperator[o];
                }
            }
            current_split += str[i];
        }

        if (seperator_match == seperator_length)
        {
            if (current_split != "")
            {
                result.push_back(current_split);
            }
            else if (!ignore_empty && current_split == "")
            {
                result.push_back(current_split);
            }
            current_split = "";
            seperator_match = 0;
        }
    }

    if (current_split != "")
    {
        result.push_back(current_split);
    }
    else if (!ignore_empty && current_split == "")
    {
        result.push_back(current_split);
    }

    return result;
}

String get_string_number(String str, bool readable_chars, bool implied_zero, int* stopped_index)
{
    String string_number;
    bool dot_used {false};
    int stopped_at {0};

    for (int i = 0; i < str.length(); i++)
    {
        char32_t crt_char = str[i];
        if(i == 0 && crt_char == '-'){
            string_number += '-';
            continue;
        }else if(i == 0 && crt_char == '+') continue;

        if(crt_char == '\'' && readable_chars) continue;
        if(crt_char == '.'){
            if(dot_used == true){
                if(readable_chars) continue;
                else{
                    stopped_at = i;
                    break;  
                }
            }else{
                bool isNotFirstCharOrPreviousWasMinus = (i-1 >= 0 && (i-1 >= 0 && str[i-1] != '-'));
                if(isNotFirstCharOrPreviousWasMinus){
                    string_number += crt_char;
                }else{
                    if(implied_zero){
                        string_number += '0';
                        string_number += crt_char;
                    }else{
                        break;
                    }
                }
                dot_used = true;
                continue;   
            }
        }

        if (crt_char == '0' || crt_char == '1' || crt_char == '2' || crt_char == '3' || 
            crt_char == '4' || crt_char == '5' || crt_char == '6' || crt_char == '7' || 
            crt_char == '8' || crt_char == '9') {
            string_number += crt_char;
        }else{
            stopped_at = i;
            break;
        }
        stopped_at = i;
    }

    if(stopped_index != nullptr){
        *stopped_index = stopped_at;
    }

    return string_number;
}