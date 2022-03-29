// Derived from:
//
// LeanTween version 2.34 - http://dentedpixel.com/developer-diary/
//
// The MIT License (MIT)
//
// Copyright (c) 2016 Russell Savage - Dented Pixel
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.


/*
TERMS OF USE - EASING EQUATIONS#
Open source under the BSD License.
Copyright (c)2001 Robert Penner
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using UnityEngine;

public static class LeanTweenEasings
{
	public static float Tween(float from, float to, float ratioPassed, LeanTweenType tweenType)
	{
		float val = 0f;
		switch( tweenType ){
			case LeanTweenType.linear:
				val = from + (to - from) * ratioPassed; break;
			case LeanTweenType.easeOutQuad:
				val = easeOutQuadOpt(from, (to - from), ratioPassed); break;
			case LeanTweenType.easeInQuad:
				val = easeInQuadOpt(from, (to - from), ratioPassed); break;
			case LeanTweenType.easeInOutQuad:
				val = easeInOutQuadOpt(from, (to - from), ratioPassed); break;
			case LeanTweenType.easeInCubic:
				val = easeInCubic(from, to, ratioPassed); break;
			case LeanTweenType.easeOutCubic:
				val = easeOutCubic(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutCubic:
				val = easeInOutCubic(from, to, ratioPassed); break;
			case LeanTweenType.easeInQuart:
				val = easeInQuart(from, to, ratioPassed); break;
			case LeanTweenType.easeOutQuart:
				val = easeOutQuart(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutQuart:
				val = easeInOutQuart(from, to, ratioPassed); break;
			case LeanTweenType.easeInQuint:
				val = easeInQuint(from, to, ratioPassed); break;
			case LeanTweenType.easeOutQuint:
				val = easeOutQuint(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutQuint:
				val = easeInOutQuint(from, to, ratioPassed); break;
			case LeanTweenType.easeInSine:
				val = easeInSine(from, to, ratioPassed); break;
			case LeanTweenType.easeOutSine:
				val = easeOutSine(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutSine:
				val = easeInOutSine(from, to, ratioPassed); break;
			case LeanTweenType.easeInExpo:
				val = easeInExpo(from, to, ratioPassed); break;
			case LeanTweenType.easeOutExpo:
				val = easeOutExpo(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutExpo:
				val = easeInOutExpo(from, to, ratioPassed); break;
			case LeanTweenType.easeInCirc:
				val = easeInCirc(from, to, ratioPassed); break;
			case LeanTweenType.easeOutCirc:
				val = easeOutCirc(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutCirc:
				val = easeInOutCirc(from, to, ratioPassed); break;
			case LeanTweenType.easeInBounce:
				val = easeInBounce(from, to, ratioPassed); break;
			case LeanTweenType.easeOutBounce:
				val = easeOutBounce(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutBounce:
				val = easeInOutBounce(from, to, ratioPassed); break;
			case LeanTweenType.easeInBack:
				val = easeInBack(from, to, ratioPassed); break;
			case LeanTweenType.easeOutBack:
				val = easeOutBack(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutBack:
				val = easeInOutBack(from, to, ratioPassed); break;
			case LeanTweenType.easeInElastic:
				val = easeInElastic(from, to, ratioPassed); break;
			case LeanTweenType.easeOutElastic:
				val = easeOutElastic(from, to, ratioPassed); break;
			case LeanTweenType.easeInOutElastic:
				val = easeInOutElastic(from, to, ratioPassed); break;
			case LeanTweenType.punch:
			case LeanTweenType.easeShake:
				throw new System.NotImplementedException();
			case LeanTweenType.easeSpring:
				val = spring(from, to, ratioPassed); break;
			case LeanTweenType.notUsed:
				val = to; break;
			default:
				{
					val = from + (to - from) * ratioPassed; break;
				}
		}
		return val;
	}

	// Tweening Functions - Thanks to Robert Penner and GFX47

	private static float easeOutQuadOpt( float start, float diff, float ratioPassed ){
		return -diff * ratioPassed * (ratioPassed - 2) + start;
	}

	private static float easeInQuadOpt( float start, float diff, float ratioPassed ){
		return diff * ratioPassed * ratioPassed + start;
	}

	private static float easeInOutQuadOpt( float start, float diff, float ratioPassed ){
		ratioPassed /= .5f;
		if (ratioPassed < 1) return diff / 2 * ratioPassed * ratioPassed + start;
		ratioPassed--;
		return -diff / 2 * (ratioPassed * (ratioPassed - 2) - 1) + start;
	}

	private static float linear(float start, float end, float val){
		return Mathf.Lerp(start, end, val);
	}

	private static float clerp(float start, float end, float val){
		float min = 0.0f;
		float max = 360.0f;
		float half = Mathf.Abs((max - min) / 2.0f);
		float retval = 0.0f;
		float diff = 0.0f;
		if ((end - start) < -half){
			diff = ((max - start) + end) * val;
			retval = start + diff;
		}else if ((end - start) > half){
			diff = -((max - end) + start) * val;
			retval = start + diff;
		}else retval = start + (end - start) * val;
		return retval;
	}

	private static float spring(float start, float end, float val ){
		val = Mathf.Clamp01(val);
		val = (Mathf.Sin(val * Mathf.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f ) + val) * (1f + (1.2f * (1f - val) ));
		return start + (end - start) * val;
	}

	private static float easeInQuad(float start, float end, float val){
		end -= start;
		return end * val * val + start;
	}

	private static float easeOutQuad(float start, float end, float val){
		end -= start;
		return -end * val * (val - 2) + start;
	}

	private static float easeInOutQuad(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val + start;
		val--;
		return -end / 2 * (val * (val - 2) - 1) + start;
	}

	private static float easeInCubic(float start, float end, float val){
		end -= start;
		return end * val * val * val + start;
	}

	private static float easeOutCubic(float start, float end, float val){
		val--;
		end -= start;
		return end * (val * val * val + 1) + start;
	}

	private static float easeInOutCubic(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val * val + start;
		val -= 2;
		return end / 2 * (val * val * val + 2) + start;
	}

	private static float easeInQuart(float start, float end, float val){
		end -= start;
		return end * val * val * val * val + start;
	}

	private static float easeOutQuart(float start, float end, float val){
		val--;
		end -= start;
		return -end * (val * val * val * val - 1) + start;
	}

	private static float easeInOutQuart(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val * val * val + start;
		val -= 2;
		return -end / 2 * (val * val * val * val - 2) + start;
	}

	private static float easeInQuint(float start, float end, float val){
		end -= start;
		return end * val * val * val * val * val + start;
	}

	private static float easeOutQuint(float start, float end, float val){
		val--;
		end -= start;
		return end * (val * val * val * val * val + 1) + start;
	}

	private static float easeInOutQuint(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * val * val * val * val * val + start;
		val -= 2;
		return end / 2 * (val * val * val * val * val + 2) + start;
	}

	private static float easeInSine(float start, float end, float val){
		end -= start;
		return -end * Mathf.Cos(val / 1 * (Mathf.PI / 2)) + end + start;
	}

	private static float easeOutSine(float start, float end, float val){
		end -= start;
		return end * Mathf.Sin(val / 1 * (Mathf.PI / 2)) + start;
	}

	private static float easeInOutSine(float start, float end, float val){
		end -= start;
		return -end / 2 * (Mathf.Cos(Mathf.PI * val / 1) - 1) + start;
	}

	private static float easeInExpo(float start, float end, float val){
		end -= start;
		return end * Mathf.Pow(2, 10 * (val / 1 - 1)) + start;
	}

	private static float easeOutExpo(float start, float end, float val){
		end -= start;
		return end * (-Mathf.Pow(2, -10 * val / 1) + 1) + start;
	}

	private static float easeInOutExpo(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return end / 2 * Mathf.Pow(2, 10 * (val - 1)) + start;
		val--;
		return end / 2 * (-Mathf.Pow(2, -10 * val) + 2) + start;
	}

	private static float easeInCirc(float start, float end, float val){
		end -= start;
		return -end * (Mathf.Sqrt(1 - val * val) - 1) + start;
	}

	private static float easeOutCirc(float start, float end, float val){
		val--;
		end -= start;
		return end * Mathf.Sqrt(1 - val * val) + start;
	}

	private static float easeInOutCirc(float start, float end, float val){
		val /= .5f;
		end -= start;
		if (val < 1) return -end / 2 * (Mathf.Sqrt(1 - val * val) - 1) + start;
		val -= 2;
		return end / 2 * (Mathf.Sqrt(1 - val * val) + 1) + start;
	}

	private static float easeInBounce(float start, float end, float val){
		end -= start;
		float d = 1f;
		return end - easeOutBounce(0, end, d-val) + start;
	}

	private static float easeOutBounce(float start, float end, float val){
		val /= 1f;
		end -= start;
		if (val < (1 / 2.75f)){
			return end * (7.5625f * val * val) + start;
		}else if (val < (2 / 2.75f)){
			val -= (1.5f / 2.75f);
			return end * (7.5625f * (val) * val + .75f) + start;
		}else if (val < (2.5 / 2.75)){
			val -= (2.25f / 2.75f);
			return end * (7.5625f * (val) * val + .9375f) + start;
		}else{
			val -= (2.625f / 2.75f);
			return end * (7.5625f * (val) * val + .984375f) + start;
		}
	}

	/*private static float easeOutBounce( float start, float end, float val, float overshoot = 1.0f ){
		end -= start;
		float baseAmt = 2.75f * overshoot;
		float baseAmt2 = baseAmt * baseAmt;
		Debug.Log("val:"+val); // 1f, 0.75f, 0.5f, 0.25f, 0.125f
		if (val < ((baseAmt-(baseAmt - 1f)) / baseAmt)){ // 0.36
			return end * (baseAmt2 * val * val) + start; // 1 - 1/1

		}else if (val < ((baseAmt-0.75f) / baseAmt)){ // .72
			val -= ((baseAmt-(baseAmt - 1f - 0.5f)) / baseAmt); // 1.25f
			return end * (baseAmt2 * val * val + .75f) + start; // 1 - 1/(4)

		}else if (val < ((baseAmt-(baseAmt - 1f - 0.5f - 0.25f)) / baseAmt)){ // .909
			val -= ((baseAmt-0.5f) / baseAmt); // 0.5
			return end * (baseAmt2 * val * val + .9375f) + start; // 1 - 1/(4*4)

		}else{ // x
			// Debug.Log("else val:"+val);
			val -= ((baseAmt-0.125f) / baseAmt); // 0.125
			return end * (baseAmt2 * val * val + .984375f) + start; // 1 - 1/(4*4*4)

		}
	}*/

	private static float easeInOutBounce(float start, float end, float val){
		end -= start;
		float d= 1f;
		if (val < d/2) return easeInBounce(0, end, val*2) * 0.5f + start;
		else return easeOutBounce(0, end, val*2-d) * 0.5f + end*0.5f + start;
	}

	private static float easeInBack(float start, float end, float val, float overshoot = 1.0f){
		end -= start;
		val /= 1;
		float s= 1.70158f * overshoot;
		return end * (val) * val * ((s + 1) * val - s) + start;
	}

	private static float easeOutBack(float start, float end, float val, float overshoot = 1.0f){
		float s = 1.70158f * overshoot;
		end -= start;
		val = (val / 1) - 1;
		return end * ((val) * val * ((s + 1) * val + s) + 1) + start;
	}

	private static float easeInOutBack(float start, float end, float val, float overshoot = 1.0f){
		float s = 1.70158f * overshoot;
		end -= start;
		val /= .5f;
		if ((val) < 1){
			s *= (1.525f) * overshoot;
			return end / 2 * (val * val * (((s) + 1) * val - s)) + start;
		}
		val -= 2;
		s *= (1.525f) * overshoot;
		return end / 2 * ((val) * val * (((s) + 1) * val + s) + 2) + start;
	}

	private static float easeInElastic(float start, float end, float val, float overshoot = 1.0f, float period = 0.3f){
		end -= start;
	
		float p = period;
		float s = 0f;
		float a = 0f;
	
		if (val == 0f) return start;

		if (val == 1f) return start + end;
	
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4f;
		}else{
			s = p / (2f * Mathf.PI) * Mathf.Asin(end / a);
		}
	
		if(overshoot>1f && val>0.6f )
			overshoot = 1f + ((1f-val) / 0.4f * (overshoot-1f));
		// Debug.Log("ease in elastic val:"+val+" a:"+a+" overshoot:"+overshoot);

		val = val-1f;
		return start-(a * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - s) * (2f * Mathf.PI) / p)) * overshoot;
	}		

	private static float easeOutElastic(float start, float end, float val, float overshoot = 1.0f, float period = 0.3f){
		end -= start;
	
		float p = period;
		float s = 0f;
		float a = 0f;
	
		if (val == 0f) return start;
	
		// Debug.Log("ease out elastic val:"+val+" a:"+a);
		if (val == 1f) return start + end;
	
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4f;
		}else{
			s = p / (2f * Mathf.PI) * Mathf.Asin(end / a);
		}
		if(overshoot>1f && val<0.4f )
			overshoot = 1f + (val / 0.4f * (overshoot-1f));
		// Debug.Log("ease out elastic val:"+val+" a:"+a+" overshoot:"+overshoot);
	
		return start + end + a * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - s) * (2f * Mathf.PI) / p) * overshoot;
	}		

	private static float easeInOutElastic(float start, float end, float val, float overshoot = 1.0f, float period = 0.3f)
	{
		end -= start;
	
		float p = period;
		float s = 0f;
		float a = 0f;
	
		if (val == 0f) return start;
	
		val = val / (1f/2f);
		if (val == 2f) return start + end;
	
		if (a == 0f || a < Mathf.Abs(end)){
			a = end;
			s = p / 4f;
		}else{
			s = p / (2f * Mathf.PI) * Mathf.Asin(end / a);
		}
	
		if(overshoot>1f){
			if( val<0.2f ){
				overshoot = 1f + (val / 0.2f * (overshoot-1f));
			}else if( val > 0.8f ){
				overshoot = 1f + ((1f-val) / 0.2f * (overshoot-1f));
			}
		}

		if (val < 1f){
			val = val-1f;
			return start - 0.5f * (a * Mathf.Pow(2f, 10f * val) * Mathf.Sin((val - s) * (2f * Mathf.PI) / p)) * overshoot;
		}
		val = val-1f;
		return end + start + a * Mathf.Pow(2f, -10f * val) * Mathf.Sin((val - s) * (2f * Mathf.PI) / p) * 0.5f * overshoot;
	}
}
